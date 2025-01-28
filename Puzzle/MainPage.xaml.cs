using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;

namespace Puzzle;

public partial class MainPage : ContentPage
{
    private static float sideWidth = 146 / 1.5f;
    private static float sideHeight = 76 / 1.5f;
    private static List<Image> puzzles = [];
    private static int gridWidth = 5;
    private static int gridHeight = 5;
    private static Grid grid;
    private static DragGestureRecognizer dragGestureRecognizer = new();
    public MainPage()
    {
        InitializeComponent();
        initializePuzzle();
        initialazeGrid();
    }

    void initializePuzzle()
    {
        int r = 0;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Image puzzle = new();
                puzzle.WidthRequest = sideWidth;
                puzzle.HeightRequest = sideHeight;
                puzzle.GestureRecognizers.Add(dragGestureRecognizer);
                puzzle.Source = $"image{r}.png";
                AbsoluteLayout.SetLayoutBounds(puzzle, new(i * sideWidth + 675, j * sideHeight , sideWidth, sideHeight));
                puzzles.Add(puzzle);
                MainLayout.Children.Add(puzzle);
                r++;
            }
        }
    }

    void initialazeGrid()
    {
        grid = new ();
        
        RowDefinitionCollection rows = [];
        for (int i = 0; i < gridHeight; i++)
        {
            rows.Add(new RowDefinition());
        }

        ColumnDefinitionCollection columns = [];
        for (int i = 0; i < gridWidth; i++)
        {
            columns.Add(new ColumnDefinition());
        }

        grid.RowDefinitions = rows;
        grid.ColumnDefinitions = columns;

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                DropGestureRecognizer dropGestureRecognizer = new();
                dropGestureRecognizer.Drop += OnDrop;
                Image box = new();
                box.BackgroundColor = Color.FromRgb(i*j + (i+j) * 25, i*j + (i + j) * 25, i*j + (i + j) * 25);
                box.GestureRecognizers.Add(dropGestureRecognizer);
                grid.Children.Add(box);
                Grid.SetRow(box, i);
                Grid.SetColumn(box, j);
            }
        }
               
        MainLayout.Children.Add(grid);
        AbsoluteLayout.SetLayoutBounds(grid, new(0, 0, sideWidth * gridWidth, sideHeight * gridHeight));
    }

    private async void OnDrop(object sender, DropEventArgs e)
    {
        var imageSource = await e.Data.GetImageAsync();
        var goalImage = sender as Image;
        if (imageSource is null || goalImage is null)
        {
            return;
        }
        goalImage.Source = imageSource;

        foreach (var view in MainLayout.Children)
        {
            Debug.Print(view.ToString());
        }
    }

    private void RefreshBtn_OnClicked(object? sender, EventArgs e)
    {
        foreach (var box in grid.Children)
        {
            var img = box as Image;
            img.Source = "";
        }
    }
}