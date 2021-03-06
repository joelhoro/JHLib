﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Deadlocked.Classes;

namespace Deadlocked
{
    public enum Action { LEFT, RIGHT, UP, DOWN };

    public class Actor
    {
        public int x, y;
        public Actor() { }
        public Actor(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Evolve(Level level, Action action) { }
    }

    public class Target: Actor
    {
        public Target(int x, int y) : base(x, y) { }
    }


    public class Player: Actor
    {
        public Player(int x, int y) : base(x, y) { }
        public override void Evolve(Level level, Action action)
        {
            switch (action)
            {
                case Action.DOWN:
                    y--;
                    break;
                case Action.UP:
                    y++;
                    break;
                case Action.LEFT:
                    x--;
                    break;
                case Action.RIGHT:
                    x++;
                    break;
            }

            x = Math.Min(Math.Max(x, 0), level.Width-1);
            y = (y+level.Height) % level.Height;
            return;
        }
    }

    public class Arrow : Player
    {
        private Action action;
        public Arrow(int x,int y, Action action) : base(x,y)
        {
            this.action = action;
        }

        public override void Evolve(Level level, Action action)
        {
            if (action == Action.UP || action == Action.DOWN)
                base.Evolve(level, this.action);
        }
    }

    public class Level
    {
        public int Width, Height;
        public List<Actor> actors = new List<Actor>();
        public event EventHandler LevelChanged;

        public Level(int Width, int Height )
        {
            this.Width = Width;
            this.Height = Height;
        }

        internal void ActionHandler(object sender, GameActionEventArgs e)
        {
            foreach(var actor in actors)
            {
                actor.Evolve(this,e.action);
            }
            LevelChanged(sender, e as EventArgs);
        }
    }

    public class GameCanvas
    {
        const int WIDTH_PIXELS = 50;
        const int HEIGHT_PIXELS = 50;
        const double CIRCLE_PERCENTAGE = 0.9;
        private Canvas canvas;
        public GameCanvas(Canvas canvas) { this.canvas = canvas; }

        public void Draw(Level level)
        {
            canvas.Children.Clear();
            DrawBorder(level);
            foreach (var actor in level.actors)
                Draw(level, actor);
        }

        public static SolidColorBrush ActorColor(Actor actor)
        {
            SolidColorBrush colorBrush = new SolidColorBrush();
            if (actor is Target)
                colorBrush.Color = Colors.Gold;
            else if (actor is Arrow)
                colorBrush.Color = Colors.Red;
            else if (actor is Player)
                colorBrush.Color = Colors.Black;
            else
                colorBrush.Color = Colors.DarkBlue;
            return colorBrush;
        }

        public void Draw(Level level, Actor actor)
        {
            var actorshape = new Ellipse();
            actorshape.Height = HEIGHT_PIXELS * CIRCLE_PERCENTAGE;
            actorshape.Width = WIDTH_PIXELS * CIRCLE_PERCENTAGE;

            actorshape.Fill = ActorColor(actor);

            Canvas.SetTop(actorshape, HEIGHT_PIXELS * (0.5 * (1-CIRCLE_PERCENTAGE) + (level.Height -1- actor.y)));
            Canvas.SetLeft(actorshape, WIDTH_PIXELS * (0.5 * (1 - CIRCLE_PERCENTAGE) + actor.x));

            canvas.Children.Add(actorshape);
        }

        public void DrawBorder(Level level)
        {
            var border = new Rectangle();
            border.Width = WIDTH_PIXELS * level.Width;
            border.Height = HEIGHT_PIXELS * level.Height;
            border.Stroke = Brushes.Black;
            Canvas.SetTop(border, 0);
            Canvas.SetLeft(border, 0);
            canvas.Children.Add(border);
        }
    }

    public class Game
    {
        public Level level;
        public event EventHandler<GameActionEventArgs> GameActionEvent;
        public event EventHandler GameStartEvent;

        public void Start()
        {
            level = new Level(9,3);
            level.actors.Add(new Player(0, 1));
            level.actors.Add(new Target(8, 1));

            int[,] actors = new int[,] { { 2, 0 }, {2,1},{ 3, 1 }, { 4, 1 }, { 5, 1 }, {6,1},{ 6, 0 }, { 4,2 } };
            for(int i = 0; i <= actors.GetUpperBound(0); i++)
                level.actors.Add(new Actor(actors[i,0],actors[i,1]));
            
            level.actors.Add(new Arrow(1, 0, Action.UP));
            GameActionEvent += level.ActionHandler;

            if (GameStartEvent != null) GameStartEvent(this, EventArgs.Empty);
        }

        internal void SendAction(Action action)
        {
            var args = new GameActionEventArgs(action, level);
            GameActionEvent(this, args);
        }
    }
}
