using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Calculator
{
    public partial class MainPage : ContentPage
    {

        private static MainPage _instance;        // Singleton-instans

        // När Instance begärs för första gången, skapas en ny instans av MainPage om ingen instans redan finns.Annars returneras den nuvarande instansen.

        public static MainPage Instance         // Offentlig åtkomstpunkt för att hämta instansen av klassen
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainPage();
                }
                return _instance;
            }
        }

        public string CurrentInput { get; private set; } = string.Empty;
        public string RunningTotal { get; private set; } = string.Empty;

        private string selectedOperator;
        private string[] operators = { "+", "-", "/", "x", "=", "Del" };// specialtecken man ska kunna trycka på 
        private string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }; // nummer som man ska kunna trycka på
        private bool resetOnNextInput = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)  // gör så min Del knapp rensar allt input
        {
            ClearInput();
        }

        private void Button_Clicked(object sender, EventArgs e) // till att kunna klicka på alla knappar på miniräknaren
        {
            var btn = sender as Button;
            var thisInput = btn.Text;

            // Anonym metod för att hantera knappklick
            Action<string> processInput = (input) =>
            {
                if (IsNumber(input))
                {
                    ProcessNumberInput(input);
                }
                else if (IsOperator(input))
                {
                    ProcessOperatorInput(input);
                }
            };

            processInput(thisInput);
        }

        private bool IsNumber(string input)//används för att avgöra om den angivna strängen representerar ett nummer.
        {
            return numbers.Contains(input);
        }

        private bool IsOperator(string input) // representerar en operator (till exempel +, -, *, /).
        {
            return operators.Contains(input);
        }

        private void ProcessNumberInput(string input)// används för att hantera inmatning av nummer
        {
            if (resetOnNextInput)
            {
                CurrentInput = input;
                resetOnNextInput = false;
            }
            else
            {
                CurrentInput += input;
            }
            result.Text = CurrentInput;
        }

        private void ProcessOperatorInput(string input) // hanterar "Del" knappen 
        {
            if (input == "Del")
            {
                DeleteButton_Clicked(null, null);
                return;
            }

            if (!string.IsNullOrEmpty(RunningTotal))
            {
                if (!string.IsNullOrEmpty(CurrentInput))
                {
                    PerformOperation(input);
                }
                else
                {
                    result.Text = "Error: Incomplete expression";
                }
            }
            else
            {
                RunningTotal = CurrentInput;
                selectedOperator = input;
                result.Text = RunningTotal + selectedOperator;
                CurrentInput = string.Empty;
            }
        }

        private async void PerformOperation(string input) // hanterar " = " alltså resultatet 
        {
            try
            {
                var ans = await PerformCalculatorAsync();

                if (input == "=")
                {
                    CurrentInput = ans.ToString();
                    result.Text = CurrentInput;
                    RunningTotal = string.Empty;
                    selectedOperator = string.Empty;
                    resetOnNextInput = true;
                }
                else
                {
                    RunningTotal = ans.ToString();
                    selectedOperator = input;
                    result.Text = RunningTotal + selectedOperator;
                    CurrentInput = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<double> PerformCalculatorAsync()
        {
            if (!double.TryParse(CurrentInput, out double currentVal))
            {
                throw new ArgumentException("Invalid input"); // denna Validerar CurrentInput
            }

            if (!double.TryParse(RunningTotal, out double runningVal))
            {
                throw new ArgumentException("Invalid running total"); // denna Validerar runningtotal
            }

            double res;

            switch (selectedOperator) // hanterar vad alla specialtecken ska göra när de trycks ned
            {
                case "+":
                    res = runningVal + currentVal;
                    break;
                case "-":
                    res = runningVal - currentVal;
                    break;
                case "x":
                    res = runningVal * currentVal;
                    break;
                case "/":
                    if (currentVal == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero");
                    }
                    res = runningVal / currentVal;
                    break;
                default:
                    res = currentVal;
                    break;
            }
            return res;
        }

        private void ClearInput()
        {
            CurrentInput = string.Empty;
            result.Text = CurrentInput;
        }

    }
}
