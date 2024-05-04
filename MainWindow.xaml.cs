using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestProject;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Component> components;

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
        // PopulateFaultGroupsTreeView();
        // InitializeComponents();
    }

    public ObservableCollection<FaultGroup> FaultGroups { get; set; }

    private void LoadData()
    {
        // Создаем список групп неисправностей
        FaultGroups = new ObservableCollection<FaultGroup>
        {
            new(
                "Система не стартует",
                new ObservableCollection<Fault>
                {
                    new(
                        "Короткое замыкание на линиях +12В, +5В, +3.3В.",
                        new Measurement("Между клеммами GND и выбранной линией питания Омметр показывает килоомы",
                            "Система не стартует, может сильно нагреваться вышедший из строя компонент или микросхема. Показатели Омметра близки к 0"),
                        "Решение: выполнить поиск неисправного элемента используя схему последовательного запуска и формирования определенных сигналов на контрольных точках. (Требуется Service Manual)."
                    ),
                    new(
                        "Неисправен южный мост",
                        new Measurement(
                            "В режиме измерения падения напряжения установить красный щуп на GND, а черным пройтись по линиям D- D+ всех USB портов, показатели не должны сильно отличаться (не более чем на десятки мВ) и находиться в примерном диапазоне от 0.450мВ до 0.7мВ",
                            "Показатели отличаются на сотни мВ или равны 0, либо есть КЗ между линиями D+ и D-"),
                        "Решение: Замена южного моста."
                    ),
                    new(
                        "Неисправность микросхемы BIOS, либо ее прошивки",
                        new Measurement(
                            "При помощи осциллографа можно увидеть сигналы (импульсы) на ножках input/output микросхемы bios, означающие обмен данными с П.У.",
                            "Отсутствуют сигналы (импульсы) на ножках input/output микросхемы bios, означающие обмен данными с П.У. отсутствует сигнал CS. Проверка осуществляется осциллографом"),
                        "Решение: Перепрошивка микросхемы BIOS, либо замена в случае ее неисправности."
                    ),
                    new(
                        "Неисправность RTC",
                        new Measurement(
                            "На часовом кварцевом резонаторе можно наблюдать осциллограмму в виде синусоиды с частотой 32768Гц",
                            "Отсутствие осциллограммы, либо несинусоидальная форма, либо неправильная частота. "),
                        "Решение: Перепайка кварцевого резонатора, поиск неисправности в системе питания, замена южного моста (после дополнительной диагностики)."
                    )
                }
            ),
            new(
                "Система стартует, но нет изображения",
                new ObservableCollection<Fault>
                {
                    new(
                        "Проблемы с разъемом видеокарты, либо линиями передачи данных",
                        new Measurement(
                            "Картинку можно увидеть на мониторе, все линии разъема видеокарты проверяются специальным тестером на обрыв",
                            "Есть обрыв в линиях передачи данных и это видно на тестере разъема видеокарты"),
                        "Решение: выполнить восстановление линий передачи данных, заменить разъем видеокарты."
                    )
                }
            ),
            new(
                "Дополнительные неисправности",
                new ObservableCollection<Fault>
                {
                    new(
                        "1. Проблемы с базовым генератором тактовой частоты или его кварцевым резонатором",
                        new Measurement("Норма: Частота 14.3МГц присутствует",
                            "Проверьте наличие частоты 14.3МГц на базовом генераторе тактовой частоты или его кварцевом резонаторе"),
                        "Замените базовый генератор тактовой частоты или его кварцевый резонатор."
                    ),
                    new(
                        "2. Нет звука или система не стартует из-за неисправного микроконтроллера звукового ядра",
                        new Measurement("Норма: Частота 25МГц присутствует",
                            "Проверьте наличие частоты 25МГц на микроконтроллере звукового ядра"),
                        "Замените микроконтроллер звукового ядра."
                    ),
                    new(
                        "3. Система не стартует из-за неисправного микроконтроллера SIO/MIO",
                        new Measurement("Норма: Система стартует после проверки мк. SIO/MIO",
                            "Проверьте последовательность запуска и сигналы на контрольных точках микроконтроллера SIO/MIO"),
                        "Замените микроконтроллер SIO/MIO."
                    )
                }
            )
        };

        // Устанавливаем источник данных для TreeView
        // faultsTreeView.ItemsSource = FaultGroups;
    }

    private void FaultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (faultsListBox.SelectedItem != null)
        {
            Fault selectedFault = (Fault)faultsListBox.SelectedItem;
            nameTextBox.Text = selectedFault.Name;
            workingTextBox.Text = selectedFault.Measurement.Working;
            notWorkingTextBox.Text = selectedFault.Measurement.NotWorking;
            solutionTextBox.Text = selectedFault.Solution;

            faultsListBox.Visibility = Visibility.Collapsed;
            faultDetailsPanel.Visibility = Visibility.Visible;
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        faultsListBox.SelectedItem = null;

        faultsListBox.Visibility = Visibility.Visible;
        faultDetailsPanel.Visibility = Visibility.Collapsed;
    }


    private void MotherboardImage_Loaded(object sender, RoutedEventArgs e)
    {
        // Используем полученные размеры для инициализации компонентов
        InitializeComponents();
    }
    
    public Rect ScaleCoordinates(Rect originalRect, double originalWidth, double originalHeight, double newWidth, double newHeight)
    {
        double scaleX = newWidth / originalWidth;
        double scaleY = newHeight / originalHeight;

        double newX = originalRect.X * scaleX;
        double newY = originalRect.Y * scaleY;

        return new Rect(newX, newY, originalRect.Width * scaleX, originalRect.Height * scaleY);
    }
    
    private void InitializeComponents()
    {
        double imageWidth = motherboardImage.ActualWidth;
        double imageHeight = motherboardImage.ActualHeight;

        // Размеры картинки
        Console.WriteLine($"Image width: {imageWidth}, Image height: {imageHeight}");
        
        double originalWidth = 349.5058400718778;
        double originalHeight = 389;

        components = new List<Component>
        {
            new Component("CPU/Северный мост", 
                ScaleCoordinates(new Rect(
                    new Point(139, 106), new Point(210, 166)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)
                ),
            new Component("ОЗУ", 
                ScaleCoordinates(new Rect(
                    new Point(248, 38), new Point(295, 235)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)
                ),
            new Component("Слот PCIe", 
                ScaleCoordinates(new Rect(
                    new Point(83, 280), new Point(211, 290)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)
                ),
            new Component("Слот PCIe", 
                ScaleCoordinates(new Rect(
                    new Point(83, 340), new Point(211, 349)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)
                ),
            new Component("Южный мост/Чипсет", 
                ScaleCoordinates(new Rect(
                    new Point(246, 290), new Point(296, 340)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)
                ),
            
            new Component("Батарейка CMOS", 
                ScaleCoordinates(new Rect(
                    new Point(131, 298), new Point(160, 327)), 
                    originalWidth, originalHeight, 
                    motherboardImage.ActualWidth, motherboardImage.ActualHeight)),
        };

        
        // Добавление неисправностей к компонентам
        components[0].AddFault(
            "CPU | Проблемы с теплопроводностью",
            "Температура в пределах нормы",
            "Температура слишком высока",
            "Применить новый слой теплопроводящей пасты (термопасты, жидкого металла)"
        );
        components[0].AddFault(
            "CPU | Проблемы с питанием",
            "Напряжение питания в пределах нормы",
            "Напряжение питания слишком низкое",
            "Проверить и, если необходимо, заменить блок питания"
        );

        components[1].AddFault(
            "ОЗУ | Проблемы с контактами",
            "Все контакты в исправном состоянии",
            "Контакты загрязнены или повреждены",
            "Очистить контакты или заменить планки памяти"
        );
        
        components[1].AddFault(
            "ОЗУ | Проблемы с напряжением питания",
            "Напряжение питания в пределах нормы",
            "Напряжение питания слишком высоко (>1.6V)",
            "Проверить и, если необходимо, заменить модули памяти"
        );

        components[2].AddFault(
            "Слот PCIe | Проблемы с сигналами",
            "Сигналы на линиях PCIe в пределах нормы",
            "Отсутствие сигналов на линиях PCIe",
            "Проверить и, если необходимо, заменить PCIe контроллер или карту расширения"
        );
        
        components[2].AddFault(
            "Слот PCIe | Проблемы с механическими повреждениями",
            "Нет видимых повреждений",
            "Есть видимые повреждения или изгибы",
            "Проверить и, если необходимо, заменить слот или контакты PCIe"
        );
        
        foreach(Fault f in components[2].Faults)
        {
            components[3].AddFault(f.Name, f.Measurement.Working, f.Measurement.NotWorking, f.Solution);
        }
        
        components[4].AddFault(
            "Южный мост/Чипсет | Отсутствие сигналов на южном мосту",
            "Все сигналы на южном мосту работают исправно",
            "Отсутствие сигналов на южном мосту может привести к неработоспособности различных подсистем",
            "Проверить и, если необходимо, заменить южный мост"
        );

        components[4].AddFault(
            "Южный мост/Чипсет | Проблемы с интерфейсами устройств на южном мосту",
            "Все интерфейсы устройств на южном мосту работают исправно",
            "Неработоспособность интерфейсов устройств на южном мосту может привести к отсутствию связи с периферийными устройствами",
            "Проверить и, если необходимо, заменить южный мост"
        );

        components[4].AddFault(
            "Южный мост/Чипсет | Проблемы с управлением питанием на южном мосту",
            "Управление питанием на южном мосту работает исправно",
            "Неработоспособность управления питанием на южном мосту может привести к проблемам с питанием периферийных устройств",
            "Проверить и, если необходимо, заменить южный мост"
        );
        
        components[0].AddFault(
            "Северный мост | Не запускается система",
            "Компьютер успешно запускается и работает.",
            "При нажатии на кнопку питания компьютер не запускается и не происходит никакой реакции.",
            "1. Проверьте подключение всех кабелей и проводов к материнской плате.\n2. Попробуйте сбросить настройки BIOS.\n3. Проверьте и, если необходимо, замените батарейку BIOS.");
        components[0].AddFault(
            "Северный мост | Периодические перезагрузки",
            "Компьютер работает стабильно и не перезагружается.",
            "Компьютер периодически перезагружается или выключается сам по себе.",
            "1. Проверьте температуру компонентов. Возможно, перегрев вызывает перезагрузки.\n2. Проведите диагностику оперативной памяти и жесткого диска.\n3. Проверьте наличие вирусов и малвари на компьютере.");
        components[0].AddFault(
            "Северный мост | Проблемы с портами",
            "Порты на материнской плате работают нормально.",
            "Порты USB или другие порты на материнской плате перестали работать или работают с ошибками.",
            "1. Проверьте подключение устройств к портам.\n2. Попробуйте обновить драйвера для портов.\n3. Возможно, требуется замена портов или самой материнской платы.");
        components[0].AddFault(
            "Северный мост | Проблемы с видеокартой",
            "Видеокарта функционирует нормально.",
            "Вывод на монитор или работа графической карты вызывают проблемы.",
            "1. Проверьте правильность подключения видеокарты к материнской плате.\n2. Попробуйте обновить драйвера для видеокарты.\n3. Проведите диагностику видеокарты.");
        components[0].AddFault(
            "Северный мост | Проблемы с оперативной памятью",
            "Оперативная память работает без ошибок.",
            "Компьютер не видит или не распознает всю установленную оперативную память.",
            "1. Проверьте правильность установки оперативной памяти.\n2. Попробуйте переставить планки памяти в другие слоты.\n3. Возможно, одна из планок памяти или слот на материнской плате неисправны.");
        
        components[5].AddFault(
            "Батарейка CMOS | Разряд или протечка батарейки CMOS", 
            "Компьютерное время, дата и настройки BIOS сохраняются при каждой перезагрузке компьютера", 
            "Компьютерное время, дата и настройки BIOS сбрасываются при каждой перезагрузке компьютера", 
            "Замените батарейку CMOS на новую");
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Получаем позицию клика относительно изображения
        Point position = e.GetPosition(motherboardImage);
        
        Console.WriteLine(position);
        
        // Определяем компонент, на который был сделан щелчок
        foreach (var component in components)
        {
            if (component.Area.Contains(position))
            {
                Console.WriteLine(component.Name);
                // Очищаем список неисправностей
                faultsListBox.ItemsSource = null;

                // Загружаем неисправности для выбранного компонента
                faultsListBox.ItemsSource = component.Faults;
                
                BackButton_Click(null, null);
                
                // Выходим из цикла, так как уже нашли компонент
                break;
            }
        }
    }
}

public class FaultGroup
{
    public FaultGroup(string name, ObservableCollection<Fault> faults)
    {
        Name = name;
        Faults = faults;
    }

    public string Name { get; set; }
    public ObservableCollection<Fault> Faults { get; set; }
}

public class Fault
{
    public Fault(string name, Measurement measurement, string solution)
    {
        Name = name;
        Measurement = measurement;
        Solution = solution;
    }

    public string Name { get; set; }
    public Measurement Measurement { get; set; }
    public string Solution { get; set; }
}

public class Measurement
{
    public Measurement(string working, string notWorking)
    {
        Working = working;
        NotWorking = notWorking;
    }

    public string Working { get; set; }
    public string NotWorking { get; set; }
}

public class Component
{
    public Component(string name, Rect area)
    {
        Name = name;
        Area = area;
        Faults = new List<Fault>();
    }

    public string Name { get; set; }
    public Rect Area { get; set; }
    public List<Fault> Faults { get; set; }

    public void AddFault(string _faultName, string _workingCondition, string _notWorkingCondition, string _solution)
    {
        Faults.Add(new Fault(_faultName, new Measurement(_workingCondition, _notWorkingCondition), _solution));
    }
}
