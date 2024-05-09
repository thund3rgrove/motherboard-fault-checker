﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestProject;

public partial class MainWindow : Window
{
    private static readonly Random random = new();

    private string currentElement;

    private static readonly Dictionary<string, Measurement[]> standardValues = new()
    {
        {
            "+12V", new Measurement[]
            {
                new("Вольтметр", 12, GenerateRandomDouble(11, 13)),
                new("Омметр", 12000, GenerateRandomDouble(11000, 13000))
            }
        },
        {
            "+3.3V", new Measurement[]
            {
                new("Вольтметр", 3.3, GenerateRandomDouble(3.0, 3.6)),
                new("Омметр", 3300, GenerateRandomDouble(3000, 3600))
            }
        },
        {
            "+5V", new Measurement[]
            {
                new("Вольтметр", 5, GenerateRandomDouble(4.8, 5.2)),
                new("Омметр", 5000, GenerateRandomDouble(4800, 5200))
            }
        },
        {
            "M_BIOS", new Measurement[]
            {
                new("Осциллограф", 1, random.NextInt64(0, 2)) // 0, 1 as bool
            }
        }
    };

    private List<Component> components = new();
    private string groundColor;
    private readonly Dictionary<string, Measurement> measurementMap = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private static double GenerateRandomDouble(double minValue, double maxValue)
    {
        return minValue + random.NextDouble() * (maxValue - minValue);
    }

    private void MotherboardImage_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeComponents();
        InitializePins();
        InitializeUSBPort();
        InitializeBIOS();
    }

    private void InitializeComponents()
    {
        var imageWidth = motherboardImage.ActualWidth;
        var imageHeight = motherboardImage.ActualHeight;

        // Размеры картинки
        Console.WriteLine($"Image width: {imageWidth}, Image height: {imageHeight}");

        var originalWidth = 349.5058400718778;
        double originalHeight = 389;

        components = new List<Component>
        {
            new("CPU/Северный мост",
                new Rect(
                    imageWidth * 0.3977043701799486, // Relative X
                    imageHeight * 0.2724935732647815, // Relative Y
                    imageWidth * 0.20314395886889458, // Relative Width
                    imageHeight * 0.15424164524421596 // Relative Height
                )
            ),
            new("ОЗУ",
                new Rect(
                    imageWidth * 0.7095732647814911, // Relative X
                    imageHeight * 0.09768637532133678, // Relative Y
                    imageWidth * 0.13447557840616967, // Relative Width
                    imageHeight * 0.5064267352185091 // Relative Height
                )
            ),
            new("Слот PCIe",
                new Rect(
                    imageWidth * 0.23747814910025708, // Relative X
                    imageHeight * 0.7197943444730077, // Relative Y
                    imageWidth * 0.3662313624678663, // Relative Width
                    imageHeight * 0.025706940874035994 // Relative Height
                )
            ),
            new("Слот PCIe",
                new Rect(
                    imageWidth * 0.23747814910025708, // Relative X
                    imageHeight * 0.8740359897172237, // Relative Y
                    imageWidth * 0.3662313624678663, // Relative Width
                    imageHeight * 0.02313624678663239 // Relative Height
                )
            ),
            new("Южный мост/Чипсет",
                new Rect(
                    imageWidth * 0.7038508997429306, // Relative X
                    imageHeight * 0.7455012853470437, // Relative Y
                    imageWidth * 0.1430591259640103, // Relative Width
                    imageHeight * 0.12853470437017997 // Relative Height
                )
            ),
            new("Батарейка CMOS",
                new Rect(
                    imageWidth * 0.37481491002570694, // Relative X
                    imageHeight * 0.7660668380462725, // Relative Y
                    imageWidth * 0.08297429305912596, // Relative Width
                    imageHeight * 0.07455012853470437 // Relative Height
                )
            )
        };
    }

    private void InitializePins()
    {
        AddVoltageButton("+3.3V", 0.9, 0.38, "3.3");
        AddVoltageButton("+5V", 0.9, 0.31, "5");
        AddVoltageButton("+12V", 0.9, 0.25, "12");
    }

    private void AddVoltageButton(string content, double leftRatio, double topRatio, string voltage)
    {
        var btn = new Button
        {
            Content = content,
            Width = 50,
            Height = 30,
            Tag = $"{voltage}V"
        };
        btn.Click += BtnVoltage_Click;
        Canvas.SetLeft(btn, motherboardImage.ActualWidth * leftRatio);
        Canvas.SetTop(btn, motherboardImage.ActualHeight * topRatio);
        pinsCanvas.Children.Add(btn);
    }

    private void InitializeUSBPort()
    {
        var usb = new Button
        {
            Content = "USB_DATA",
            Width = 50,
            Height = 30,
            Tag = "USB_DATA"
        };

        usb.Click += USBPortButton_Click;
        Canvas.SetLeft(usb, motherboardImage.ActualWidth * 0.07);
        Canvas.SetTop(usb, motherboardImage.ActualHeight * 0.36);
        usbCanvas.Children.Add(usb);
    }
    
    private void InitializeBIOS()
    {
        var btn = new Button
        {
            Content = "M_BIOS",
            Width = 40,
            Height = 30,
            Tag = "M_BIOS"
        };

        btn.Click += BiosClick;
        Canvas.SetLeft(btn, motherboardImage.ActualWidth * 0.35);
        Canvas.SetTop(btn, motherboardImage.ActualHeight * 0.57);
        usbCanvas.Children.Add(btn);
    }

    private void BtnVoltage_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        string lineName = btn.Content?.ToString();

        if (lineName == null)
            return;

        currentElement = lineName;
        HandleMeasurement(lineName);
    }

    private void USBPortButton_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;
        if (btn == null) return;
        string element = btn.Tag?.ToString();

        if (element == null)
            return;

        currentElement = element;
        HandleMeasurement(element);
    }

    private void BiosClick(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        string element = btn.Tag?.ToString();

        if (element == null)
            return;

        currentElement = element;
        HandleMeasurement(element);
    }

    
    private void HandleMeasurement(string element)
    {
        if (standardValues.ContainsKey(element))
        {
            Measurement[] measurements = standardValues[element];
            string selectedTool = GetSelectedTool();

            foreach (Measurement measurement in measurements)
            {
                OscillographSineImage.Visibility = Visibility.Collapsed;
                OscillographPlainImage.Visibility = Visibility.Collapsed;

                if (measurement.Instrument == selectedTool)
                {
                    switch (selectedTool)
                    {
                        case "Омметр":
                            MeasurementText.Text = $"Измерение сопротивления на {element}: {measurement.GeneratedValue} Ohm";
                            break;
                        case "Вольтметр":
                            double deviation = 0.05 * measurement.StandardValue;
                            MeasurementText.Text = $"Напряжение на {element}: {measurement.GeneratedValue}V\nДопустимое отклонение: ±{deviation}V";
                            break;
                        case "Осциллограф":
                            string signal;
                            if (measurement.GeneratedValue == 1)
                            {
                                signal = "Синусоидальный сигнал";
                                OscillographSineImage.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                signal = "Отсутствие импульсов";
                                OscillographPlainImage.Visibility = Visibility.Visible;
                            }
                            
                            // Simulating oscilloscope: displaying different states
                            MeasurementText.Text = $"{element}: {signal}";
                            
                            break;
                        default:
                            MeasurementText.Text = $"Невозможно выполнить измерение на \"{element}\" с помощью \"{selectedTool}\".";
                            break;
                    }
                    return;
                }
            }
            MeasurementText.Text = $"Не найден подходящий инструмент для измерения {element}.";
        }
        else
        {
            MeasurementText.Text = $"Для {element} не заданы эталонные значения.";
        }
    }
    
    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Получаем позицию клика относительно изображения
        var position = e.GetPosition(motherboardImage);
        Console.WriteLine($"Click at {position}");

        var imageWidth = motherboardImage.ActualWidth;
        var imageHeight = motherboardImage.ActualHeight;
        var relativePosition = new Point(e.GetPosition(motherboardImage).X / imageWidth,
            e.GetPosition(motherboardImage).Y / imageHeight);
        Console.WriteLine($"Относительное положение клика: X={relativePosition.X}, Y={relativePosition.Y}");

        // Определяем компонент, на который был сделан щелчок
        foreach (var component in components)
            if (component.Area.Contains(position))
            {
                MeasureComponent(component);
                // Выходим из цикла, так как уже нашли компонент
                break;
            }
    }

    private void MeasureComponent(Component component)
    {
        // Здесь вы можете выполнить измерения для выбранного компонента
        // Например, вы можете отобразить измерения в каком-то элементе управления
        // или вывести сообщение с результатами измерений
        // Для примера, просто выведем имя компонента в консоль
        Console.WriteLine($"Выполнены измерения для компонента: {component.Name}");
    }

    private void RadioButton_Group_Checked(object sender, RoutedEventArgs e)
    {
        var selectedRadioButton = (RadioButton)sender;
        if (selectedRadioButton != null)
        {
            groundColor = selectedRadioButton.Content.ToString();
            Console.WriteLine($"Выбранный цвет земли (GND): {groundColor}");
        }
    }
    
    private string GetSelectedTool()
    {
        foreach (var item in LogicalTreeHelper.GetChildren(toolbar))
            if (item is RadioButton radioButton && radioButton.IsChecked == true)
                return radioButton.Content.ToString();
        return null; // If no tool is selected
    }

private void WorkingButton_Click(object sender, RoutedEventArgs e)
{
    if (currentElement == null)
    {
        // Handle the case where no element is selected
        MessageBox.Show("No element selected.", "Error");
        return;
    }

    Button btn = (Button)sender;
    bool isYesClicked = btn.Tag.ToString() == "yesBtn" ? true : false;
    
    if (standardValues.ContainsKey(currentElement))
    {
        Measurement[] measurements = standardValues[currentElement];
        foreach (Measurement measurement in measurements)
        {
            // if (measurement.Instrument == GetSelectedTool()) // TODO: uncomment if dont want to allow making diagnosis without choosing correct tool
            {
                switch (measurement.Instrument)
                {
                    case "Осциллограф":
                        if (measurement.StandardValue == measurement.GeneratedValue)
                        {
                            ShowResultMessage("BIOS is working correctly.", "Верно!", "Неверно!", isYesClicked);
                        }
                        else
                        {
                            ShowResultMessage("BIOS is not working correctly.", "Неверно!", "Верно!", isYesClicked);
                        }
                        return;
                    case "Вольтметр":
                        // For voltage measurements, check if the generated value is within 5% deviation
                        double deviation = 0.05 * measurement.StandardValue;
                        if (Math.Abs(measurement.StandardValue - measurement.GeneratedValue) <= deviation)
                        {
                            ShowResultMessage("Voltage measurement is within acceptable deviation.", "Верно!", "Неверно!", isYesClicked);
                        }
                        else
                        {
                            ShowResultMessage("Voltage measurement is not within acceptable deviation.", "Неверно!", "Верно!", isYesClicked);
                        }
                        return;
                    case "Омметр":
                        // For resistance measurements, check if the measured resistance falls within an acceptable range
                        if (Math.Abs(measurement.StandardValue - measurement.GeneratedValue) <= 100)
                        {
                            ShowResultMessage("Resistance measurement is within acceptable range.", "Верно!", "Неверно!", true);
                        }
                        else
                        {
                            ShowResultMessage("Resistance measurement is not within acceptable range.", "Неверно!", "Верно!", false);
                        }
                        return;                        
                }
            }
        }
    }
    else
    {
        // Handle the case where the selected element doesn't have standard values defined
        MessageBox.Show($"No standard values defined for {currentElement}.", "Error");
    }
    
}    
    private void ShowResultMessage(string message, string correctTitle, string incorrectTitle, bool isCorrect)
    {
        if (isCorrect)
        {
            MessageBox.Show($"{correctTitle} {message}", correctTitle);
        }
        else
        {
            MessageBox.Show($"{incorrectTitle} {message}", incorrectTitle);
        }
    }

}

public class Component
{
    public Component(string name, Rect area)
    {
        Name = name;
        Area = area;
    }

    public string Name { get; set; }
    public Rect Area { get; set; }
}

public class Measurement
{
    public Measurement(string instrument, double standardValue, double generatedValue)
    {
        Instrument = instrument;
        StandardValue = standardValue;
        GeneratedValue = generatedValue;
    }

    public string Instrument { get; set; }
    public double StandardValue { get; set; }
    public double GeneratedValue { get; set; }
}