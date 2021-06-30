using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace projekt_serwatko
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Karty do wylosowania
        string[] karty = new string[52] { "images/2C.png", "images/2D.png", "images/2H.png", "images/2S.png", "images/3C.png", "images/3D.png", "images/3H.png", "images/3S.png", "images/4C.png", "images/4D.png", "images/4H.png", "images/4S.png", "images/5C.png", "images/5D.png", "images/5H.png", "images/5S.png", "images/6C.png", "images/6D.png", "images/6H.png", "images/6S.png", "images/7C.png", "images/7D.png", "images/7H.png", "images/7S.png", "images/8C.png", "images/8D.png", "images/8H.png", "images/8S.png", "images/9C.png", "images/9D.png", "images/9H.png", "images/9S.png", "images/10C.png", "images/10D.png", "images/10H.png", "images/10S.png", "images/AC.png", "images/AD.png", "images/AH.png", "images/AS.png", "images/JC.png", "images/JD.png", "images/JH.png", "images/JS.png", "images/KC.png", "images/KD.png", "images/KH.png", "images/KS.png", "images/QC.png", "images/QD.png", "images/QH.png", "images/QS.png" };
        // Wartości kart do wylosowania
        int[] wartosci = new int[52] { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        
        int pieniadze = 1000, kwota_zaklad, krupier_wynik, gracz_wynik;
        string krupier2_karta, gracz_hit1_karta, gracz_hit2_karta;
        int krupier2_wartosc, gracz_hit1_wartosc, gracz_hit2_wartosc;
        Random rand = new Random();
        bool hit_bool, double_down, instrukcja;
        public bool hit2_bool;

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wyjść z gry?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    wyjdz_button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void surrender_button_Click(object sender, RoutedEventArgs e)
        {
            wynik_canvas.Visibility = Visibility.Visible; exit_button.Visibility = Visibility.Collapsed;
            wygrana_label.Text = "Poddałeś się!";

            // Odjęcie połowy zakładu od sumy gracza
            pieniadze -= kwota_zaklad / 2;
            kwota_gracza_label.Content = "Twoja suma: " + pieniadze + " zł";

            // Zablokowanie przycisków po zakończeniu rozgrywki
            hit_button.IsEnabled = false; stand_button.IsEnabled = false; double_down_button.IsEnabled = false; surrender_button.IsEnabled = false;
        }

        private void instrukcja_button_Click(object sender, RoutedEventArgs e)
        {
            if (instrukcja == false)
            {
                instrukcja_canvas.Visibility = Visibility.Visible; strzalka.Visibility = Visibility.Visible;
                instrukcja = true;
            }
            else
            {
                instrukcja_canvas.Visibility = Visibility.Collapsed; strzalka.Visibility = Visibility.Collapsed;
                instrukcja = false;
            }
        }

        private void wynik()
        {
            // Jeśli wynik gracza wynosi więcej niż 21 - wywołanie opcji stand i zakończenie gry przegraną
            if (gracz_wynik > 21)
                stand_button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void double_down_button_Click(object sender, RoutedEventArgs e)
        {
            // Pomnożenie zakładu przez 2
            kwota_zaklad *= 2;
            zaklad_label.Content = "Kwota zakładu: " + kwota_zaklad + " zł";
            hit2_bool = true; double_down = true;

            // Zablokowanie opcji double down
            double_down_button.IsEnabled = false;
        }

        private void stand_button_Click(object sender, RoutedEventArgs e)
        {
            // Odsłonięcie drugiej karty krupiera i dodanie jej wartości
            krupier2.Source = new BitmapImage(new Uri(krupier2_karta, UriKind.Relative));
            krupier_wynik += krupier2_wartosc;
            krupier_wynik_label.Content = krupier_wynik;

            wynik_canvas.Visibility = Visibility.Visible; exit_button.Visibility = Visibility.Collapsed;

            if (gracz_wynik > krupier_wynik && gracz_wynik <= 21 || krupier_wynik > 21 && gracz_wynik <= 21)
            {
                // Jeśli wynik gracza jest większy od wyniku krupiera i nie przekracza 21, lub krupier uzyskał więcej niż 21, a gracz nie - wygrana
                wygrana_label.Text = "Wygrałeś!";
                // Dodanie kwoty zakładu do sumy gracza
                pieniadze += kwota_zaklad;
                kwota_gracza_label.Content = "Twoja suma: " + pieniadze + " zł";
            }
            else if(gracz_wynik == krupier_wynik)
            {
                // Jeśli gracz uzyskał tyle samo punktów co krupier - remis
                wygrana_label.Text = "Remis!";
                kwota_gracza_label.Content = "Twoja suma: " + pieniadze + " zł";
            }
            else
            {
                // W przeciwnym wypadku - przegrana
                wygrana_label.Text = "Przegrałeś!";
                // Odjęcie kwoty zakładu od sumy gracza
                pieniadze -= kwota_zaklad;
                kwota_gracza_label.Content = "Twoja suma: " + pieniadze + " zł";
            }

            // Jeśli suma gracza jest mniejsza niż jeden - zbankruptowanie i zablokowanie opcji dalszej gry
            if (pieniadze < 1)
                dalej_button.IsEnabled = false;

            // Zablokowanie przycisków po zakończeniu rozgrywki
            hit_button.IsEnabled = false; stand_button.IsEnabled = false; double_down_button.IsEnabled = false; surrender_button.IsEnabled = false;
        }

        private void dalej_button_Click(object sender, RoutedEventArgs e)
        {
            wynik_canvas.Visibility = Visibility.Collapsed; gra_canvas.Visibility = Visibility.Collapsed;

            // Wywołanie przycisku graj
            graj_button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            // Zakrycie drugiej karty krupiera i kart dodatkowych gracza
            krupier2.Source = new BitmapImage(new Uri("images/blue_back.png", UriKind.Relative));
            gracz3.Visibility = Visibility.Hidden; gracz4.Visibility = Visibility.Hidden;

            // Reset wyników oraz odblokowanie przycisków
            krupier_wynik = 0; gracz_wynik = 0;
            double_down_button.IsEnabled = true; hit_button.IsEnabled = true;
            hit_bool = false; hit2_bool = false; double_down = false;
        }

        private void wyjdz_button_Click(object sender, RoutedEventArgs e)
        {
            wynik_canvas.Visibility = Visibility.Collapsed; gra_canvas.Visibility = Visibility.Collapsed; zaklad_canvas.Visibility = Visibility.Collapsed;
            gracz3.Visibility = Visibility.Hidden; gracz4.Visibility = Visibility.Hidden; exit_button.Visibility = Visibility.Collapsed;
            tytul_canvas.Visibility = Visibility.Visible;

            // Reset sumy gracza, wyników i schowanie drugiej karty krupiera
            pieniadze = 1000;
            krupier2.Source = new BitmapImage(new Uri("images/blue_back.png", UriKind.Relative));
            krupier_wynik = 0; gracz_wynik = 0;

            hit_bool = false; hit2_bool = false; double_down = false;
            double_down_button.IsEnabled = true; hit_button.IsEnabled = true; dalej_button.IsEnabled = true;
        }

        private void graj_button_Click(object sender, RoutedEventArgs e)
        {
            tytul_canvas.Visibility = Visibility.Collapsed; zaklad_canvas.Visibility = Visibility.Visible; exit_button.Visibility = Visibility.Visible;

            // Wypisanie sumy gracza oraz ustawienie jej jako maksymalną wartość slider'a
            kwota_label.Content = "Masz " + pieniadze + " zł";
            zaklad_slider.Maximum = pieniadze;
            
            hit_button.IsEnabled = true; stand_button.IsEnabled = true; double_down_button.IsEnabled = true; surrender_button.IsEnabled = true;
        }

        private void graj2_button_Click(object sender, RoutedEventArgs e)
        {
            // Losowanie pozycji kart
            List<int> dostepne_karty = Enumerable.Range(0, 52).ToList();
            // Lista do przechowywania wartości wylosowanych kart
            List<int> losowanie = new List<int>();

            zaklad_canvas.Visibility = Visibility.Collapsed;
            kwota_zaklad = Int32.Parse(zaklad_textbox.Text);
            
            gra_canvas.Visibility = Visibility.Visible; exit_button.Visibility = Visibility.Visible;

            // Wylosowanie 6 kart, które zostaną rozdane krupierowi oraz graczowi
            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(0, dostepne_karty.Count());
                losowanie.Add(dostepne_karty[index]);
                // Usunięcie wylosowanych kart, aby nie powtarzały się w tej samej rozgrywce
                dostepne_karty.RemoveAt(index);
            }

            // Przyporządkowanie krupierowi obrazków kart oraz ich wartości
            krupier1.Source = new BitmapImage(new Uri(karty[losowanie[0]], UriKind.Relative));
            krupier_wynik += wartosci[losowanie[0]];
            krupier_wynik_label.Content = krupier_wynik + " + ?";

            // Przyporządkowanie graczowi obrazków kart oraz ich wartości
            gracz1.Source = new BitmapImage(new Uri(karty[losowanie[1]], UriKind.Relative));
            gracz2.Source = new BitmapImage(new Uri(karty[losowanie[2]], UriKind.Relative));
            gracz_wynik += wartosci[losowanie[1]] + wartosci[losowanie[2]];
            gracz_wynik_label.Content = gracz_wynik;

            zaklad_label.Content = "Kwota zakładu: " + kwota_zaklad + " zł";
            kwota_gracza_label.Content = "Twoja suma: " + pieniadze + " zł";

            // Przyporządkowanie obrazków oraz wartości kart jeszcze nie odsłoniętych
            krupier2_karta = karty[losowanie[5]]; krupier2_wartosc = wartosci[losowanie[5]];
            gracz_hit1_karta = karty[losowanie[4]]; gracz_hit1_wartosc = wartosci[losowanie[4]];
            gracz_hit2_karta = karty[losowanie[3]]; gracz_hit2_wartosc = wartosci[losowanie[3]];

            // Sprawdzenie, czy wynik gracza nie przekracza 21
            wynik();
        }

        private void hit_button_Click(object sender, RoutedEventArgs e)
        {
            if (hit_bool == true && double_down == false)
            {
                // Jeśli już raz użyto opcji hit i nie użyto double down - odsłonięcie 4 karty gracza i dodanie jej wartości
                gracz4.Source = new BitmapImage(new Uri(gracz_hit1_karta, UriKind.Relative));
                gracz4.Visibility = Visibility.Visible;
                gracz_wynik += gracz_hit1_wartosc;
                gracz_wynik_label.Content = gracz_wynik;
                hit2_bool = true;

                // Wywołanie opcji stand, gdyż gracz nie może już nic innego zrobić
                stand_button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                // Sprawdzenie, czy wynik gracza nie przekracza 21
                wynik();
            }
            else
            {
                // Jeśli nie użyto jeszcze opcji hit - odsłonięcie 3 karty gracza i dodanie jej wartości
                gracz3.Source = new BitmapImage(new Uri(gracz_hit2_karta, UriKind.Relative));
                gracz3.Visibility = Visibility.Visible;
                gracz_wynik += gracz_hit2_wartosc;
                gracz_wynik_label.Content = gracz_wynik;
                hit_bool = true;

                // Sprawdzenie, czy wynik gracza nie przekracza 21
                wynik();

                // Zablokowanie opcji double down
                double_down_button.IsEnabled = false;
                // Jeśli opcja double down została już użyta - zablokowanie opcji hit
                if (double_down == true)
                    hit_button.IsEnabled = false;
            }
        }
    }
}
