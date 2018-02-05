using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using Prism.Commands;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CharacterReplacement
{

    public class CharacterReplacementViewModel : INotifyPropertyChanged
    {
        #region Fields        

        private string _inputText;
        private string _outputText;

        

        /// <summary>
        /// A string of characters to use for parsing effects. Must be non-null at the beginning, due to using methods with it later, so we instantiate as an empty string.
        /// </summary>
        private string _parsingCharacters = "";

        private ObservableHashSet<char> _changedChars = new ObservableHashSet<char>();
        private ObservableHashSet<string> _changedStrings = new ObservableHashSet<string>();

        #endregion Fields

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RemoveCharactersCommand
        {
            get; set;
        }

        public CharacterReplacementViewModel()
        {
            _changedChars.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => NotifyPropertyChanged(nameof(ChangedChars));
            _changedChars.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => NotifyPropertyChanged(nameof(ChangedCharsString));
            _changedStrings.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => NotifyPropertyChanged(nameof(ChangedStrings));
            _changedStrings.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => NotifyPropertyChanged(nameof(ChangedStringsString));

            RemoveCharactersCommand = new DelegateCommand(KeepOnlyChars);

        }

        #region Properties
        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(InputTextList));
                NotifyPropertyChanged("_inputText");
                NotifyPropertyChanged("InputText");
            }
        }

        public string[] InputTextList
        {
            get { return _inputText.Split('\n')??new string[]{""}; }
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

        public string ParsingCharacters
        {
            get { return _parsingCharacters; }
            set
            {
                _parsingCharacters = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableHashSet<char> ChangedChars
        {
            get { return _changedChars; }
            set
            {
                _changedChars = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChangedCharsString));
            }
        }

        public string ChangedCharsString
        {
            get { return new string(ChangedChars.ToArray()); }
        }

        public ObservableHashSet<string> ChangedStrings
        {
            get { return _changedStrings; }
            set
            {
                _changedStrings = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChangedStringsString));
            }
        }

        public string ChangedStringsString
        {
            get { return String.Join("", ChangedStrings); }
        }
        #endregion Properties

        #region Text Changer Methods
         
        public void RemoveCharacters()
        {  
            foreach (char c in ParsingCharacters)
            {
                foreach(string s in InputTextList)
                {
                    if (s.Contains(c))
                    {
                        ChangedChars.Add(c);
                        ChangedStrings.Add(s);          
                    }
                }
            }

            OutputText = InputText;

            foreach (char c in ParsingCharacters)
            {
                OutputText = OutputText.Replace(c.ToString(), "");
            }
        }

        public void KeepOnlyChars()
        {
            foreach (string s in InputTextList)
            {
                foreach (char c in s)
                {
                    if (!ParsingCharacters.Contains(c) && c!='\n' && c!='\r')
                        {
                            ChangedChars.Add(c);
                            ChangedStrings.Add(s);
                            continue;
                        }
                    }
                }
            

            OutputText = "";

            foreach (char c in InputText)
            {
                if (ParsingCharacters.Contains(c) || c=='\n' || c=='\r')
                    OutputText = OutputText + c;
            }
        }

        public static IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        #endregion Text Changer Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}