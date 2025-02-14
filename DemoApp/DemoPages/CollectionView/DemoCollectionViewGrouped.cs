using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DemoApp;

public class DemoCollectionViewGrouped : CollectionView
{
    public DemoCollectionViewGrouped()
    {
        ItemsSource = new List<AnimalGroup> {
            new AnimalGroup("Mammals", new List<Animal> {
                new Animal { Name = "Elephant", Description = "Elephantidae" },
                new Animal { Name = "Lion", Description = "Panthera leo" },
                new Animal { Name = "Zebra", Description = "Equus zebra" }
            }),
            new AnimalGroup("Reptiles", new List<Animal> {
                new Animal { Name = "Alligator", Description = "Alligatoridae" },
                new Animal { Name = "Crocodile", Description = "Crocodylidae" },
                new Animal { Name = "Chameleon", Description = "Chamaeleonidae" }
            }),
        };
        ItemTemplate = new DataTemplate(typeof(AnimalView));
        GroupHeaderTemplate = new DataTemplate(() => {
            var label = new Label { FontAttributes = FontAttributes.Bold };
            label.SetBinding(Label.TextProperty, "Name");
            return label;
        });
        IsGrouped = true;
        BackgroundColor = Colors.GhostWhite;
    }
}

public class Animal
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class AnimalGroup(string name, List<Animal> animals) : List<Animal>(animals)
{
    public string Name { get; private set; } = name;
}

public class AnimalView : ContentView
{
    public AnimalView()
    {
        var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
        nameLabel.SetBinding(Label.TextProperty, nameof(Animal.Name), BindingMode.OneWay);

        var descriptionLabel = new Label();
        descriptionLabel.SetBinding(Label.TextProperty, nameof(Animal.Description), BindingMode.OneWay);

        Content = new StackLayout {
            Orientation = StackOrientation.Horizontal,
            Spacing = 5,
            Children = {
                nameLabel,
                descriptionLabel
            }
        };
    }
}