﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deadlocked
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();

            KeyDown += new KeyEventHandler(OnButtonKeyPress);

            game = new Game(canvas);
            game.Start();
        }

        private void OnButtonKeyPress(object sender, KeyEventArgs e)
        {
            var keys = new Dictionary<Key,Action> {
                { Key.Down, Action.DOWN },
                { Key.Up, Action.UP },
                { Key.Left, Action.LEFT },
                { Key.Right, Action.RIGHT},
            };

            if ( keys.ContainsKey(e.Key) )
            {
                game.Send(keys[e.Key]);            
            }
        }



    }
}