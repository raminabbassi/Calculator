using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public partial class MathQuizPage : ContentPage
    {
        private List<MathQuestion> _questions;

        public MathQuizPage()
        {
            InitializeComponent();
            GenerateQuestions();
            QuestionsCollectionView.ItemsSource = _questions;
        }

        private void GenerateQuestions()
        {
            var random = new Random();
            _questions = new List<MathQuestion>();

            for (int i = 0; i < 12; i++)
            {
                int num1 = random.Next(1, 10);
                int num2 = random.Next(1, 10);
                _questions.Add(new MathQuestion
                {
                    Question = $"{num1} x {num2}",
                    CorrectAnswer = num1 * num2
                });
            }
        }

        private async void OnCheckAnswersClicked(object sender, EventArgs e)
        {
            int correctCount = _questions.Count(q => q.IsCorrect);
            ResultLabel.Text = $"Du fick {correctCount} av 12 rätt!";
            ResultLabel.IsVisible = true;
        }
        private async void restart(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("mathquiz");
        }


    }

    public class MathQuestion
    {
        public string Question { get; set; }
        public int CorrectAnswer { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect => int.TryParse(Answer, out int userAnswer) && userAnswer == CorrectAnswer;
    }
}