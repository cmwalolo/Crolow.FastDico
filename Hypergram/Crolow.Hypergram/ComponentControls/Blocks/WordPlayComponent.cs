using Crolow.FastDico.Utils;
using Kalow.Hypergram.Core.Solver.Utils;
using Kalow.Hypergram.Logic.Models.GamePlay;
using Kalow.Hypergram.Logic.Models.GameSetup;
using MauiBlazorWeb.Shared.ComponentBases.Base;
using MauiBlazorWeb.Shared.Interfaces;
using MauiBlazorWeb.Shared.Interfaces.HyperGram;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

public class WordPlayComponent : ModalComponent
{
    public int CurrentBoard = 0;
    public int CurrentPoints = 0;

    public HypergramWordContainer BoardWordContainer { get; set; }
    public HypergramWordContainer PlayerWordContainer { get; set; }
    public HypergramRound[] WordContainers { get; set; }

    public HypergramGameRound CurrentGameRound { get; set; }

    [Inject]
    IMessageService messageService { get; set; }

    [Inject]
    public IHypergramGameService hypergramGameService { get; set; }

    [Parameter]
    public HypergramRoom Room { get; set; }
    public bool IsInvalid { get; set; }

    protected async override void OnInitialized()
    {
    }

    public void SetWordContainers(int currentBoard, HypergramWordContainer board, HypergramWordContainer player)
    {
        CurrentBoard = currentBoard;
        IsInvalid = false;
        BoardWordContainer = board;
        PlayerWordContainer = player;

        WordContainers = new HypergramRound[]
            {new HypergramRound(Room.Board.BoardConfig), new HypergramRound(Room.Board.BoardConfig),new HypergramRound(Room.Board.BoardConfig) };

        WordContainers[0].SetWord(board.GetWord());
        WordContainers[1].SetWord(player.GetWord());
        WordContainers[2].SetWord("");
        CalculateCurrentPoints();

    }
    public void Check()
    {
        var word = WordContainers[2].GetWord();
        CurrentPoints = 0;
        IsInvalid = true;
        if (hypergramGameService.CheckWord(word))
        {
            var boardLen = WordContainers[2].CountFromBoard();
            var rackLen = WordContainers[2].CountFromRack();

            if (boardLen == BoardWordContainer.WordLength && rackLen > 0)
            {
                IsInvalid = false;
                StateHasChanged();
            }
        }

        if (!IsInvalid)
        {
            CurrentGameRound = new HypergramGameRound()
            {
                Points = CalculateCurrentPoints(),
                RemainingRack = WordContainers[1].GetWord(),
                WordPlayed = word,
                LettersPlayed = WordContainers[2].GetFromRack(),
                RackPlayed = CurrentBoard,
                PreviousWord = BoardWordContainer.Word
            };

            StateHasChanged();
        }

    }
    public override void Close()
    {
        base.Close();
    }

    public void Clear()
    {
        CurrentGameRound = null;
    }

    public void Validate()
    {
        if (CurrentGameRound == null)
        {
            Check();
        }

        if (!IsInvalid)
        {
            messageService.SendMessage(new MauiBlazorWeb.Shared.Models.MessageModel()
            {
                Type = MauiBlazorWeb.Shared.Models.MessageModel.MessageType.RoundIsPlayed,
                MessageObject = CurrentGameRound
            });

            CurrentGameRound = null;
            this.Close();
        }
    }

    public void Next()
    {
        messageService.SendMessage(new MauiBlazorWeb.Shared.Models.MessageModel()
        {
            Type = MauiBlazorWeb.Shared.Models.MessageModel.MessageType.BoardRackSelectNext,
            MessageObject = BoardWordContainer
        });
    }

    public void Previous()
    {
        messageService.SendMessage(new MauiBlazorWeb.Shared.Models.MessageModel()
        {
            Type = MauiBlazorWeb.Shared.Models.MessageModel.MessageType.BoardRackSelectPrevious,
            MessageObject = BoardWordContainer
        });
    }

    public int CalculateCurrentPoints()
    {
        var wc = WordContainers[2];

        var points = 0;
        for (int x = 0; x < wc.WordLen(); x++)
        {
            if (wc.Joker(x) != TilesUtils.JokerByte)
            {
                var tile = wc.GetTile(x);
                points += Room.Board.BoardConfig.GetTilePoints(tile);
            }
        }

        CurrentPoints = points * wc.WordLen() - BoardWordContainer.GetScore();
        return CurrentPoints;
    }

    public void KeyPress(KeyboardEventArgs e)
    {
        if (e.Key.Length == 1)
        {
            if (char.IsLetter(e.Key[0]))
            {
                AddLetter(char.ToUpper(e.Key[0]));
                CalculateCurrentPoints();
                StateHasChanged();
            }
        }

        switch (e.Key)
        {
            case "Enter":
                Check();
                break;
            case "Backspace":
                RemoveLastLetter();
                CalculateCurrentPoints();
                StateHasChanged();
                break;
            case "ArrowUp":
                Previous();
                break;
            case "ArrowDown":
                Next();
                break;
        }
    }

    private void RemoveLastLetter()
    {
        var len = WordContainers[2].WordLen();
        if (len > 0)
        {
            var orig = WordContainers[2].GetTileOrigin(len - 1);
            var letter = WordContainers[2].GetTile(len - 1);
            WordContainers[2].RemoveRightToRack();
            if (orig == 1)
            {
                WordContainers[0].AddRightFromBoard(letter);
            }
            else
            {
                WordContainers[1].AddRightFromRack(letter, 0);
            }
        }
    }
    private void AddLetter(char letter)
    {
        char l = (char)Room.Board.BoardConfig.GetTileCode(letter);

        if (WordContainers[0].GetAndRemoveLetter(l))
        {
            WordContainers[2].AddRightFromBoard(l);
        }
        else if (WordContainers[1].GetAndRemoveLetter(l))
        {
            WordContainers[2].AddRightFromRack(l, 0);
        }
    }
}
