using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CharacterReplacement
{

    public class CharacterReplacementViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private string _inputText = "fred";
        private string _outputText;

        public CharacterReplacementViewModel()
        {

        }

        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(InputTextList));
            }
        }

        public string[] InputTextList
        {
            get { return _inputText.Split('\n'); }
            set { InputText = String.Join("\n", value); }
        }

        public string OutputText
        {
            get { return _outputText; }
            set
            {
                _outputText = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(OutputTextList));
            }

        }

        public string[] OutputTextList
        {
            get { return _outputText.Split('\n'); }
            set { OutputText = String.Join("\n", value); }
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}