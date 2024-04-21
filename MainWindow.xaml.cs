using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FaultGroup> FaultGroups { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            PopulateFaultGroupsTreeView();
        }

        private void LoadData()
        {
            // Создаем список групп неисправностей
            FaultGroups = new ObservableCollection<FaultGroup>
            {
                new FaultGroup(
                    "Система не стартует",
                    new ObservableCollection<Fault>
                    {
                        new Fault(
                            "Короткое замыкание на линиях +12В, +5В, +3.3В.",
                            new Measurement("Между клеммами GND и выбранной линией питания Омметр показывает килоомы", 
                                "Система не стартует, может сильно нагреваться вышедший из строя компонент или микросхема. Показатели Омметра близки к 0"),
                            "Решение: выполнить поиск неисправного элемента используя схему последовательного запуска и формирования определенных сигналов на контрольных точках. (Требуется Service Manual)."
                        ),
                        new Fault(
                            "Неисправен южный мост",
                            new Measurement("В режиме измерения падения напряжения установить красный щуп на GND, а черным пройтись по линиям D- D+ всех USB портов, показатели не должны сильно отличаться (не более чем на десятки мВ) и находиться в примерном диапазоне от 0.450мВ до 0.7мВ", 
                                "Показатели отличаются на сотни мВ или равны 0, либо есть КЗ между линиями D+ и D-"),
                            "Решение: Замена южного моста."
                        ),
                        new Fault(
                            "Неисправность микросхемы BIOS, либо ее прошивки",
                            new Measurement("При помощи осциллографа можно увидеть сигналы (импульсы) на ножках input/output микросхемы bios, означающие обмен данными с П.У.", "Отсутствуют сигналы (импульсы) на ножках input/output микросхемы bios, означающие обмен данными с П.У. отсутствует сигнал CS. Проверка осуществляется осциллографом"),
                            "Решение: Перепрошивка микросхемы BIOS, либо замена в случае ее неисправности."
                        ),
                        new Fault(
                            "Неисправность RTC",
                            new Measurement("На часовом кварцевом резонаторе можно наблюдать осциллограмму в виде синусоиды с частотой 32768Гц", 
                                "Отсутствие осциллограммы, либо несинусоидальная форма, либо неправильная частота. "),
                            "Решение: Перепайка кварцевого резонатора, поиск неисправности в системе питания, замена южного моста (после дополнительной диагностики)."
                        ),
                    }
                ),
                new FaultGroup(
                    "Система стартует, но нет изображения",
                    new ObservableCollection<Fault>
                    {
                        new Fault(
                            "Проблемы с разъемом видеокарты, либо линиями передачи данных",
                            new Measurement("Картинку можно увидеть на мониторе, все линии разъема видеокарты проверяются специальным тестером на обрыв", 
                                "Есть обрыв в линиях передачи данных и это видно на тестере разъема видеокарты"),
                            "Решение: выполнить восстановление линий передачи данных, заменить разъем видеокарты."
                        ),
                    }
                ),
                new FaultGroup(
                    "Дополнительные неисправности",
                    new ObservableCollection<Fault>
                    {
                        new Fault(
                            "1. Проблемы с базовым генератором тактовой частоты или его кварцевым резонатором",
                            new Measurement("Норма: Частота 14.3МГц присутствует", "Проверьте наличие частоты 14.3МГц на базовом генераторе тактовой частоты или его кварцевом резонаторе"),
                            "Замените базовый генератор тактовой частоты или его кварцевый резонатор."
                        ),
                        new Fault(
                            "2. Нет звука или система не стартует из-за неисправного микроконтроллера звукового ядра",
                            new Measurement("Норма: Частота 25МГц присутствует", "Проверьте наличие частоты 25МГц на микроконтроллере звукового ядра"),
                            "Замените микроконтроллер звукового ядра."
                        ),
                        new Fault(
                            "3. Система не стартует из-за неисправного микроконтроллера SIO/MIO",
                            new Measurement("Норма: Система стартует после проверки мк. SIO/MIO", "Проверьте последовательность запуска и сигналы на контрольных точках микроконтроллера SIO/MIO"),
                            "Замените микроконтроллер SIO/MIO."
                        )
                    }
                )
            };
            
            // Устанавливаем источник данных для TreeView
            faultsTreeView.ItemsSource = FaultGroups;
        }
        
        private void PopulateFaultGroupsTreeView()
        {
            faultsTreeView.ItemsSource = FaultGroups; // Устанавливаем источник данных для TreeView
        }
        
        private void FaultsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Обработка выбора элемента в дереве
            if (faultsTreeView.SelectedItem is FaultGroup selectedGroup)
            {
                // Если выбрана группа, выведите ее имя или что-то еще
                // MessageBox.Show($"Выбрана группа: {selectedGroup.Name}");
            }
            else if (faultsTreeView.SelectedItem is Fault selectedFault)
            {
                // Если выбрана конкретная неисправность, выведите ее имя или что-то еще
                // MessageBox.Show($"Выбрана неисправность: {selectedFault.Name}");

                // Установить содержимое текстовых полей на основе выбранной неисправности
                descriptionTextBox.Text = selectedFault.Name;
                measurementWorkingTextBox.Text = selectedFault.Measurement.Working;
                measurementNotWorkingTextBox.Text = selectedFault.Measurement.NotWorking;
                solutionTextBox.Text = selectedFault.Solution;
            }
        }

    }

    public class FaultGroup
    {
        public string Name { get; set; }
        public ObservableCollection<Fault> Faults { get; set; }

        public FaultGroup(string name, ObservableCollection<Fault> faults)
        {
            Name = name;
            Faults = faults;
        }
    }

    public class Fault
    {
        public string Name { get; set; }
        public Measurement Measurement { get; set; }
        public string Solution { get; set; }

        public Fault(string name, Measurement measurement, string solution)
        {
            Name = name;
            Measurement = measurement;
            Solution = solution;
        }
    }
    
    public class Measurement
    {
        public string Working { get; set; }
        public string NotWorking { get; set; }

        public Measurement(string working, string notWorking)
        {
            Working = working;
            NotWorking = notWorking;
        }
    }
    
    
}
