package main

import (
	"fmt"
	"image/color"
	"os"
	"strings"

	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/canvas"
	"fyne.io/fyne/v2/container"
	"fyne.io/fyne/v2/dialog"
	"fyne.io/fyne/v2/theme"
	"fyne.io/fyne/v2/widget"
)

const (
	Degree     = 40  // Степень многочлена (размер регистра)
	BitsToShow = 108 // Количество бит для отображения
)

// Taps для многочлена x^40 + x^21 + x^19 + x^2 + 1
var Taps = []int{40, 21, 19, 2}

// CustomTheme - пользовательская тема для приложения
type CustomTheme struct {
	fyne.Theme
}

func (t CustomTheme) Color(name fyne.ThemeColorName, variant fyne.ThemeVariant) color.Color {
	switch name {
	case theme.ColorNameBackground:
		return color.RGBA{18, 18, 24, 255}
	case theme.ColorNameForeground:
		return color.RGBA{255, 255, 255, 255}
	case theme.ColorNamePrimary:
		return color.RGBA{100, 108, 255, 255}
	default:
		return t.Theme.Color(name, variant)
	}
}

// LFSR - линейный сдвиговый регистр с обратной связью
type LFSR struct {
	state []byte // состояние регистра (0 или 1)
	taps  []int  // отводы для обратной связи
}

// NewLFSR создает новый LFSR с заданным начальным состоянием
func NewLFSR(seed string, taps []int) *LFSR {
	state := make([]byte, Degree)
	for i := 0; i < Degree && i < len(seed); i++ {
		if seed[i] == '1' {
			state[i] = 1
		}
	}
	return &LFSR{state: state, taps: taps}
}

// NextBit генерирует следующий бит (как в примере)
func (l *LFSR) NextBit() byte {
	// Выходной бит - первый элемент state (младший бит)
	outBit := l.state[0]

	feedback := l.state[38] // позиция 40
	feedback ^= l.state[19] // позиция 21
	feedback ^= l.state[17] // позиция 19
	feedback ^= l.state[0]  // позиция 2

	// Сдвиг влево (как в примере)
	for i := 0; i < Degree-1; i++ {
		l.state[i] = l.state[i+1]
	}

	// Помещаем feedback в старший бит (как в примере)
	l.state[Degree-1] = feedback

	return outBit
}

// ProcessData обрабатывает данные (шифрование/дешифрование)
// Возвращает: outputData, firstKeyBytes, lastKeyBytes
func (l *LFSR) ProcessData(inputData []byte, limitBytes int) (outputData []byte, firstKey, lastKey []byte) {
	outputData = make([]byte, len(inputData))
	firstKey = make([]byte, 0, limitBytes)
	lastKeyRing := make([]byte, limitBytes)

	for i, b := range inputData {
		var cryptByte byte = 0
		var keyByte byte = 0

		for bitIdx := 7; bitIdx >= 0; bitIdx-- {
			keyBit := l.NextBit()
			fileBit := (b >> bitIdx) & 1
			cryptBit := fileBit ^ keyBit

			keyByte |= (keyBit << bitIdx)
			cryptByte |= (cryptBit << bitIdx)
		}

		outputData[i] = cryptByte

		if i < limitBytes {
			firstKey = append(firstKey, keyByte)
		}
		if len(inputData) > 0 {
			lastKeyRing[i%limitBytes] = keyByte
		}
	}

	if len(inputData) <= limitBytes {
		lastKey = firstKey
	} else {
		lastKey = make([]byte, limitBytes)
		for i := 0; i < limitBytes; i++ {
			lastKey[i] = lastKeyRing[(len(inputData)+i)%limitBytes]
		}
	}

	return outputData, firstKey, lastKey
}

// toBitStr преобразует байты в битовую строку
func toBitStr(data []byte) string {
	var sb strings.Builder
	for _, b := range data {
		sb.WriteString(fmt.Sprintf("%08b", b))
	}
	return sb.String()
}

// getFirstBits возвращает первые BitsToShow бит
func getFirstBits(data []byte, bitsCount int) string {
	s := toBitStr(data)
	if len(s) > bitsCount {
		return s[:bitsCount]
	}
	return s
}

// getLastBits возвращает последние BitsToShow бит
func getLastBits(data []byte, bitsCount int) string {
	s := toBitStr(data)
	if len(s) > bitsCount {
		return s[len(s)-bitsCount:]
	}
	return s
}

// formatBits форматирует битовую строку с пробелами каждые 8 бит
func formatBits(bitStr string) string {
	var result strings.Builder
	for i, ch := range bitStr {
		if i > 0 && i%8 == 0 {
			result.WriteString(" ")
		}
		result.WriteRune(ch)
	}
	return result.String()
}

func StartUI() {
	myApp := app.NewWithID("com.unf0r9ecryptography_lfsr40")
	myApp.Settings().SetTheme(CustomTheme{Theme: theme.DefaultTheme()})

	myWindow := myApp.NewWindow(fmt.Sprintf("Потоковое шифрование (LFSR %d) - Многочлен: x^%d + x^%d + x^%d + x^%d + 1",
		Degree, Taps[0], Taps[1], Taps[2], Taps[3]))

	var filePathToRead string
	var resultBytes []byte
	var firstKeyBytes, lastKeyBytes []byte

	// Заголовок
	titleText := canvas.NewText("🔐 LFSR Stream Cipher", color.RGBA{100, 108, 255, 255})
	titleText.TextSize = 24
	titleText.TextStyle = fyne.TextStyle{Bold: true}

	labelPathFromFile := widget.NewLabel("Файл для чтения не выбран")
	labelPathFromFile.Wrapping = fyne.TextWrapWord

	// Поля вывода
	entryOriginal := widget.NewMultiLineEntry()
	entryOriginal.SetMinRowsVisible(10)
	entryOriginal.Wrapping = fyne.TextWrapBreak
	entryOriginal.TextStyle = fyne.TextStyle{Monospace: true}

	entryKey := widget.NewMultiLineEntry()
	entryKey.SetMinRowsVisible(8)
	entryKey.Wrapping = fyne.TextWrapBreak
	entryKey.TextStyle = fyne.TextStyle{Monospace: true}

	entryResult := widget.NewMultiLineEntry()
	entryResult.SetMinRowsVisible(10)
	entryResult.Wrapping = fyne.TextWrapBreak
	entryResult.TextStyle = fyne.TextStyle{Monospace: true}

	// Поле ввода ключа
	keyEntry := widget.NewEntry()
	keyEntry.PlaceHolder = fmt.Sprintf("Введите %d бит начального состояния (только 0 и 1)...", Degree)
	keyEntry.OnChanged = func(s string) {
		var filtered string
		for _, char := range s {
			if char == '0' || char == '1' {
				filtered += string(char)
			}
		}
		if len(filtered) > Degree {
			filtered = filtered[:Degree]
		}
		if s != filtered {
			keyEntry.SetText(filtered)
		}
	}

	// Кнопка сохранения
	btnSaveAs := widget.NewButtonWithIcon("💾 Сохранить результат", theme.DocumentSaveIcon(), func() {
		if len(resultBytes) == 0 {
			dialog.ShowInformation("Ошибка", "Нет данных для сохранения!", myWindow)
			return
		}
		dialog.ShowFileSave(func(writer fyne.URIWriteCloser, err error) {
			if err != nil || writer == nil {
				return
			}
			defer writer.Close()
			writer.Write(resultBytes)
			dialog.ShowInformation("Успех", "Файл успешно сохранен!", myWindow)
		}, myWindow)
	})
	btnSaveAs.Disable()

	// Основная функция обработки
	process := func() {
		if len(keyEntry.Text) != Degree {
			dialog.ShowInformation("ОШИБКА", fmt.Sprintf("Ключ должен состоять ровно из %d бит!\nСейчас введено: %d бит",
				Degree, len(keyEntry.Text)), myWindow)
			return
		}
		if filePathToRead == "" {
			dialog.ShowInformation("ОШИБКА", "Файл для чтения не выбран!", myWindow)
			return
		}

		inputBytes, err := os.ReadFile(filePathToRead)
		if err != nil {
			dialog.ShowError(err, myWindow)
			return
		}

		// Создаем LFSR и обрабатываем данные
		lfsr := NewLFSR(keyEntry.Text, Taps)

		// limitBytes = ceil(BitsToShow / 8) + запас
		limitBytes := (BitsToShow / 8) + 2
		outputBytes, firstKey, lastKey := lfsr.ProcessData(inputBytes, limitBytes)
		resultBytes = outputBytes
		firstKeyBytes = firstKey
		lastKeyBytes = lastKey

		// Формируем вывод для исходного файла
		var uiOrig, uiKey, uiCrypt string

		// Исходный файл: первые и последние BitsToShow бит
		firstOrigBits := getFirstBits(inputBytes, BitsToShow)
		lastOrigBits := getLastBits(inputBytes, BitsToShow)

		if len(inputBytes)*8 <= BitsToShow*2 {
			uiOrig = formatBits(firstOrigBits)
		} else {
			uiOrig = fmt.Sprintf("📌 ПЕРВЫЕ %d БИТ:\n%s\n\n... (пропущено %d бит) ...\n\n📌 ПОСЛЕДНИЕ %d БИТ:\n%s",
				BitsToShow, formatBits(firstOrigBits),
				len(inputBytes)*8-BitsToShow*2,
				BitsToShow, formatBits(lastOrigBits))
		}

		// Ключевой поток: первые и последние BitsToShow бит
		firstKeyBits := getFirstBits(firstKeyBytes, BitsToShow)
		lastKeyBits := getLastBits(lastKeyBytes, BitsToShow)

		if len(firstKeyBytes)*8 <= BitsToShow {
			uiKey = formatBits(firstKeyBits)
		} else {
			uiKey = fmt.Sprintf("📌 ПЕРВЫЕ %d БИТ КЛЮЧА:\n%s\n\n📌 ПОСЛЕДНИЕ %d БИТ КЛЮЧА:\n%s",
				BitsToShow, formatBits(firstKeyBits), BitsToShow, formatBits(lastKeyBits))
		}

		// Результат: первые и последние BitsToShow бит
		firstResultBits := getFirstBits(outputBytes, BitsToShow)
		lastResultBits := getLastBits(outputBytes, BitsToShow)

		if len(outputBytes)*8 <= BitsToShow*2 {
			uiCrypt = formatBits(firstResultBits)
		} else {
			uiCrypt = fmt.Sprintf("📌 ПЕРВЫЕ %d БИТ:\n%s\n\n... (пропущено %d бит) ...\n\n📌 ПОСЛЕДНИЕ %d БИТ:\n%s",
				BitsToShow, formatBits(firstResultBits),
				len(outputBytes)*8-BitsToShow*2,
				BitsToShow, formatBits(lastResultBits))
		}

		entryOriginal.SetText(uiOrig)
		entryKey.SetText(uiKey)
		entryResult.SetText(uiCrypt)

		btnSaveAs.Enable()

		dialog.ShowInformation("Успех",
			fmt.Sprintf("✅ Файл успешно обработан!\n📊 Размер: %d байт (%d бит)\n🔍 Показано: первые и последние %d бит",
				len(inputBytes), len(inputBytes)*8, BitsToShow),
			myWindow)
	}

	// Кнопки
	buttonEncrypt := widget.NewButtonWithIcon("🔒 Зашифровать / Расшифровать", theme.ConfirmIcon(), process)
	buttonEncrypt.Importance = widget.HighImportance

	buttonClear := widget.NewButtonWithIcon("🗑️ Очистить всё", theme.DeleteIcon(), func() {
		entryOriginal.SetText("")
		entryKey.SetText("")
		entryResult.SetText("")
		keyEntry.SetText("")
		filePathToRead = ""
		resultBytes = nil
		firstKeyBytes = nil
		lastKeyBytes = nil
		labelPathFromFile.SetText("Файл для чтения не выбран")
		btnSaveAs.Disable()
	})

	btnOpenFileToRead := widget.NewButtonWithIcon("📂 Выбрать файл", theme.FileIcon(), func() {
		dialog.ShowFileOpen(func(reader fyne.URIReadCloser, err error) {
			if err != nil {
				dialog.ShowError(err, myWindow)
				return
			}
			if reader != nil {
				filePathToRead = reader.URI().Path()
				fileName := reader.URI().Name()
				fileInfo, _ := os.Stat(filePathToRead)
				fileSize := fileInfo.Size()
				labelPathFromFile.SetText(fmt.Sprintf("✅ %s\n📁 %s\n📊 Размер: %d байт (%d бит)",
					fileName, filePathToRead, fileSize, fileSize*8))
				reader.Close()
			}
		}, myWindow)
	})

	// Информационная панель
	polynomialText := canvas.NewText(fmt.Sprintf("x^%d + x^%d + x^%d + x^%d + 1", Taps[0], Taps[1], Taps[2], Taps[3]), color.RGBA{100, 108, 255, 255})
	polynomialText.TextSize = 14
	polynomialText.TextStyle = fyne.TextStyle{Monospace: true}

	infoCard := widget.NewCard("📖 Информация о системе", "", container.NewVBox(
		widget.NewLabelWithStyle("📐 Примитивный многочлен:", fyne.TextAlignLeading, fyne.TextStyle{Bold: true}),
		polynomialText,
		widget.NewSeparator(),
		widget.NewLabelWithStyle(fmt.Sprintf("⚙️ Степень регистра: %d бит", Degree), fyne.TextAlignLeading, fyne.TextStyle{Bold: true}),
		widget.NewLabelWithStyle(fmt.Sprintf("📊 Отображается по %d бит (первые и последние)", BitsToShow), fyne.TextAlignLeading, fyne.TextStyle{Italic: true}),
	))

	keyCard := widget.NewCard("🔐 Ключ шифрования", "", container.NewVBox(
		keyEntry,
		container.NewHBox(
			widget.NewButtonWithIcon("📋 Вставить пример", theme.ContentCopyIcon(), func() {
				exampleKey := strings.Repeat("1", Degree)
				keyEntry.SetText(exampleKey)
			}),
			widget.NewButtonWithIcon("🧹 Очистить ключ", theme.DeleteIcon(), func() {
				keyEntry.SetText("")
			}),
		),
	))

	fileCard := widget.NewCard("📁 Работа с файлом", "", container.NewVBox(
		btnOpenFileToRead,
		labelPathFromFile,
	))

	actionCard := widget.NewCard("⚡ Действия", "", container.NewHBox(buttonEncrypt, buttonClear, btnSaveAs))

	// Вкладки с результатами
	tabs := container.NewAppTabs(
		container.NewTabItemWithIcon("📄 Исходный файл", theme.FileIcon(), entryOriginal),
		container.NewTabItemWithIcon("🔑 Ключевой поток", theme.InfoIcon(), entryKey),
		container.NewTabItemWithIcon("🔒 Результат", theme.ConfirmIcon(), entryResult),
	)
	tabs.SetTabLocation(container.TabLocationTop)

	resultsCard := widget.NewCard("📊 Результаты обработки", "", tabs)

	// Сборка интерфейса
	header := container.NewVBox(
		container.NewCenter(titleText),
		widget.NewSeparator(),
	)

	content := container.NewVBox(
		header,
		infoCard,
		keyCard,
		fileCard,
		actionCard,
		resultsCard,
	)

	paddedContent := container.NewPadded(content)
	scrollContainer := container.NewVScroll(paddedContent)
	scrollContainer.SetMinSize(fyne.NewSize(1000, 700))

	myWindow.SetContent(scrollContainer)
	myWindow.Resize(fyne.NewSize(1100, 850))
	myWindow.CenterOnScreen()
	myWindow.ShowAndRun()
}

func main() {
	StartUI()
}
