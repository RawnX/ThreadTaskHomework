using System;
using System.Windows;
using System.Threading;
using ThreadTaskHomework.Helper;
using ThreadTaskHomework.Commads;
using System.Collections.ObjectModel;


namespace ThreadTaskHomework.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand ResumeCommand { get; set; }
        public RelayCommand KeyDownCommand { get; set; }


        public Thread Thread { get; set; }



        private ObservableCollection<string> _myListBox;
        public ObservableCollection<string> myListBox
        {
            get { return _myListBox; }
            set { _myListBox = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> _cryptographyListBox;
        public ObservableCollection<string> CryptographyListBox
        {
            get { return _cryptographyListBox; }
            set { _cryptographyListBox = value; OnPropertyChanged(); }
        }


        private string _TxtBox;
        public string TxtBox
        {
            get { return _TxtBox; }
            set { _TxtBox = value; OnPropertyChanged(); }
        }





        public MainViewModel()
        {

            myListBox = new ObservableCollection<string>();
            CryptographyListBox = new ObservableCollection<string>();


            // Full Complete
            KeyDownCommand = new RelayCommand((o) =>
            {
                Thread keyDownThread = new Thread(() =>
                {
                    if (TxtBox != string.Empty)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            myListBox.Add(TxtBox);
                        });
                    }
                    else
                        MessageBox.Show("Metni daxil edin", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                    TxtBox = string.Empty;
                });

                keyDownThread.Start();
            });



            PlayCommand = new RelayCommand((o) =>
            {
                Thread PlayThread = new Thread(() =>
                {
                    Thread = new Thread(() =>
                    {
                        ObservableCollection<string> temp = new ObservableCollection<string>(myListBox);
                        string deletelist = string.Empty;

                        foreach (var item in temp)
                        {
                            deletelist = item;

                            Thread threadRemove = new Thread(() =>
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                                {
                                    myListBox.Remove(deletelist);
                                    MessageBox.Show($"{deletelist} Listboxdan silinir", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                                });
                            });


                            Thread threadAdd = new Thread(() =>
                            {
                                threadRemove.Join();
                                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                                {
                                    CryptographyListBox.Add(EncrytionHelper.sha256(deletelist));
                                });

                            });

                            threadRemove.Start();
                            Thread.Sleep(2500);
                            threadAdd.Start();
                        };
                    });

                    Thread.Start();

                    if (myListBox.Count == 0)
                    {
                        Thread?.Abort();
                    }

                });

                PlayThread.Start();
            });


            StopCommand = new RelayCommand((o) =>
            {
                Thread.Abort();
                MessageBox.Show("Stoped", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            PauseCommand = new RelayCommand((o) =>
            {
                try
                {
                    Thread.Suspend();
                    MessageBox.Show("Suspend", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

            ResumeCommand = new RelayCommand((o) =>
            {
                try
                {
                    Thread.Resume();
                    MessageBox.Show("Resume", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

        }
    }
}

