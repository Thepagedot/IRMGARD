using System;
using System.Collections.Generic;
using System.Linq;

using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class LetterpuzzleSelectConceptFragment : SelectConceptFragment
    {
        const int Cols = 8;
        const int Rows = 8;
        const int Padding = 4;

        List<Concept> words;

        protected override void InitBaseLayoutView(View layoutView)
        {
            var llTaskItemRows = layoutView.FindViewById<LinearLayout>(Resource.Id.llTaskItemRows);
            llTaskItemRows.SetPadding(ToPx(Padding), ToPx(Padding), ToPx(Padding), ToPx(Padding));
            llTaskItemRows.SetBackgroundColor(Android.Graphics.Color.White);

            base.InitBaseLayoutView(layoutView);
        }

        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            var random = new Random();

            words = exercise.TaskItems[0];
            words.Shuffle();

            exercise.TaskItems = new List<List<Concept>>();
            foreach (var concept in words)
            {
                var word = concept as Word;

                var row = new List<Concept>();
                var start = random.Next(0, (Cols - word.Text.Length) + 1);
                while (row.Count < Cols)
                {
                    if (row.Count == start)
                    {
                        row.AddRange(ToLetters(word.Text));
                    }
                    else
                    {
                        row.Add(GetRandomLetter(random));
                    }
                }
                exercise.TaskItems.Add(row);
            }

            for (int i = 0; i < (Rows - words.Count); i++)
            {
                exercise.TaskItems.Insert(random.Next(0, exercise.TaskItems.Count), CreateDummyRow(random));
            }
        }

        Letter GetRandomLetter(Random random)
        {
            return CreateLetter(false, char.ToString((char)('A' + random.Next(0, 26))));
        }

        List<Concept> CreateDummyRow(Random random)
        {
            var letters = new List<Concept>();
            for (int i = 0; i < Cols; i++)
            {
                letters.Add(GetRandomLetter(random));
            }

            return letters;
        }

        List<Concept> ToLetters(string word)
        {
            var letters = new List<Concept>();
            foreach (var letter in word.AsEnumerable())
            {
                letters.Add(CreateLetter(true, char.ToString(letter)));
            }

            return letters;
        }

        Letter CreateLetter(bool isSolution, string text)
        {
            var letter = new Letter();
            if (isSolution)
            {
                letter.IsSolution = true;
            }
            else
            {
                letter.IsOption = true;
            }
            letter.Text = text;
            letter.ShowAsPlainText = true;
            letter.TextSize = IsSmallHeight() ? 18 : 26;

            return letter;
        }

        protected override View CreateAndInitConceptView(Concept concept)
        {
            var container = base.CreateAndInitConceptView(concept) as ViewGroup;

            // Adjust padding for letter views rendered as plain text
            container.GetChildAt(0).SetPadding(ToPx(Padding), 0, ToPx(Padding), 0);

            return container;
        }

        protected override bool IsTextCardCallback(BaseText concept)
        {
            return false;
        }

        protected override bool CheckUserInteracted()
        {
            bool hasUserInteracted = false;

            // Check if the user is done and for any words found
            for (int i = 0; i < llTaskItemRows.ChildCount; i++)
            {
                var view = (ViewGroup)llTaskItemRows.GetChildAt(i);
                var llTaskItemRow = view.FindViewById<LinearLayout>(Resource.Id.llTaskItemRow);
                var chars = new List<char>();
                var viewsFound = new List<View>();
                for (int k = 0; k < llTaskItemRow.ChildCount; k++)
                {
                    var containerView = llTaskItemRow.GetChildAt(k) as ViewGroup;
                    var contentView = containerView.GetChildAt(0);
                    if (GetTag<Drawable>(contentView, Resource.Id.selected_tag_key) != null)
                    {
                        var concept = GetTag<Concept>(contentView, Resource.Id.concept_tag_key) as Letter;
                        chars.Add(char.Parse(concept.Text));
                        viewsFound.Add(contentView);

                        hasUserInteracted = true;
                    }
                }

                var word = new string(chars.ToArray());
                if (words.Any(w => (w as Word).Text.Equals(word)))
                {
                    viewsFound.ForEach(v => v.SetBackgroundResource(Resource.Drawable.highlighted_background));
                }
            }

            return hasUserInteracted;
        }
    }
}