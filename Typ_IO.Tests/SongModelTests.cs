using Xunit;
using BasisJaar2.Models;
using System;

namespace Typ_IO.Tests;

public class SongModelTests
{
    [Fact]
    public void Song_KanGecreerWorden()
    {
        // Arrange & Act
        var song = new Song
        {
            Titel = "Test Song",
            Artiest = "Test Artiest",
            Moeilijkheid = "Easy",
            Bpm = 120,
            Lengte = new TimeSpan(0, 3, 30)
        };

        // Assert
        Assert.Equal("Test Song", song.Titel);
        Assert.Equal("Test Artiest", song.Artiest);
        Assert.Equal(120, song.Bpm);
    }

    [Fact]
    public void LengteFormatted_3Min34Sec_Geeft3_34()
    {
        // Arrange
        var song = new Song
        {
            Lengte = new TimeSpan(0, 3, 34)
        };

        // Act
        var result = song.LengteFormatted;

        // Assert
        Assert.Equal("3:34", result);
    }

    [Fact]
    public void LengteFormatted_1Min5Sec_Geeft1_05()
    {
        // Arrange
        var song = new Song
        {
            Lengte = new TimeSpan(0, 1, 5)
        };

        // Act
        var result = song.LengteFormatted;

        // Assert
        Assert.Equal("1:05", result);
    }

    [Fact]
    public void BpmText_120_GeeftBPM120()
    {
        // Arrange
        var song = new Song { Bpm = 120 };

        // Act
        var result = song.BpmText;

        // Assert
        Assert.Equal("BPM 120", result);
    }

    [Fact]
    public void DifficultyInitial_Medium_GeeftM()
    {
        // Arrange
        var song = new Song { Moeilijkheid = "Medium" };

        // Act
        var result = song.DifficultyInitial;

        // Assert
        Assert.Equal("M", result);
    }

    [Fact]
    public void DifficultyInitial_Easy_GeeftE()
    {
        // Arrange
        var song = new Song { Moeilijkheid = "Easy" };

        // Act
        var result = song.DifficultyInitial;

        // Assert
        Assert.Equal("E", result);
    }

    [Fact]
    public void DifficultyInitial_Hard_GeeftH()
    {
        // Arrange
        var song = new Song { Moeilijkheid = "Hard" };

        // Act
        var result = song.DifficultyInitial;

        // Assert
        Assert.Equal("H", result);
    }

    [Fact]
    public void AllePropperties_KunnenWordenIngesteld()
    {
        // Arrange & Act
        var song = new Song
        {
            Titel = "Enemy",
            Artiest = "Imagine Dragons",
            Moeilijkheid = "Medium",
            Bpm = 120,
            Lengte = new TimeSpan(0, 3, 34)
        };

        // Assert
        Assert.NotNull(song.Titel);
        Assert.NotNull(song.Artiest);
        Assert.NotNull(song.Moeilijkheid);
        Assert.True(song.Bpm > 0);
        Assert.True(song.Lengte.TotalSeconds > 0);
    }
}