using System;
using System.Collections.Generic;
using System.Linq;

using IRMGARD.Models;
using IRMGARD.Shared;

namespace IRMGARD
{
    public class DefArtFPSCFragment : FourPicturesSelectConceptFragment
    {
        protected override void TransformTaskItems()
        {
            var selectConcept = Lesson.DeepCopy();

            var derConcepts = selectConcept.NamedConcepts["Der"];
            var dieConcepts = selectConcept.NamedConcepts["Die"];
            var dasConcepts = selectConcept.NamedConcepts["Das"];

            var fourPictures = new List<Concept>();

            var random = new Random();
            switch (random.Next(0, 3))
            {
                case 0:
                    AddToFourPictures(fourPictures, dieConcepts, derConcepts);
                    break;
                case 1:
                    AddToFourPictures(fourPictures, derConcepts, dieConcepts);
                    break;
                case 2:
                    AddToFourPictures(fourPictures, dasConcepts, dieConcepts);
                    break;
            }

            BuildFourPictures(null, fourPictures, null);
        }

        void AddToFourPictures(List<Concept> fourPictures, List<Concept> solutionConcepts, List<Concept> optionConcepts)
        {
            // Add solution item
            var solutionConcept = solutionConcepts.PickRandomItems(1).FirstOrDefault();
            solutionConcept.IsSolution = true;
            fourPictures.Add(solutionConcept);

            // Add options
            fourPictures.AddRange(optionConcepts.PickRandomItems(3).Select(item => { item.IsOption = true; return item; }));
        }
    }

    public class ShortLongVowelFPSCFragment : FourPicturesSelectConceptFragment
    {
        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            // Random select between short or long vowels
            var random = new Random();
            int solutionsRowIdx = 1;
            int optionsRowIdx = 3;
            if (random.Next(0, 2) == 1)
            {
                solutionsRowIdx = 3;
                optionsRowIdx = 1;
            }

            var fourPictures = new List<Concept>();

            // Add solution item
            var solutionConcept = exercise.TaskItems[solutionsRowIdx].PickRandomItems(1).FirstOrDefault();
            solutionConcept.IsSolution = true;
            fourPictures.Add(solutionConcept);

            // Add options
            fourPictures.AddRange(exercise.TaskItems[optionsRowIdx].PickRandomItems(3).Select(item => { item.IsOption = true; return item; }));

            BuildFourPictures(null, fourPictures, exercise.TaskItems[solutionsRowIdx - 1]);
        }
    }

    public class FourPicturesSelectConceptFragment : SelectConceptFragment
    {
        protected override void OnCreateViewConfig()
        {
            SelectedBackgroundResourceId = Resource.Drawable.highlighted_background;
        }

        protected override void TransformTaskItems()
        {
            exercise = exercise.DeepCopy();

            switch (exercise.TaskItems.Count)
            {
                case 1:
                    BuildFourPictures(null, exercise.TaskItems[0], null);
                    break;
                case 2:
                    BuildFourPictures(null, exercise.TaskItems[0], exercise.TaskItems[1]);
                    break;
                case 3:
                    BuildFourPictures(exercise.TaskItems[0], exercise.TaskItems[1], exercise.TaskItems[2]);
                    break;
            }
        }

        protected void BuildFourPictures(List<Concept> headerItems, List<Concept> fourPictureItems, List<Concept> footerItems)
        {
            if (exercise != null && exercise.OptionItems != null && exercise.OptionItems.Count > 0 && fourPictureItems.Count < 4)
            {
                fourPictureItems.AddRange(exercise.OptionItems.PickRandomItems(4 - fourPictureItems.Count).Select(item => { item.IsOption = true; return item; }));
            }

            if (Lesson.OptionItems != null && Lesson.OptionItems.Count > 0 && fourPictureItems.Count < 4)
            {
                fourPictureItems.AddRange(Lesson.OptionItems.PickRandomItems(4 - fourPictureItems.Count).Select(item => { item.IsOption = true; return item; }));
            }

            fourPictureItems.ForEach(c => (c as Picture).Size = IsSmallHeight() ? 100 : 120);
            fourPictureItems.Shuffle();

            if (exercise == null)
            {
                exercise = new SelectConceptExercise();
            }
            exercise.TaskItems = new List<List<Concept>>();
            if (headerItems != null && headerItems.Count > 0)
            {
                exercise.TaskItems.Add(headerItems);
                exercise.TaskItems.Add(new List<Concept>() { new Models.Space() });
            }
            exercise.TaskItems.Add(fourPictureItems.GetRange(0, 2));
            exercise.TaskItems.Add(fourPictureItems.GetRange(2, 2));
            if (footerItems != null && footerItems.Count > 0)
            {
                exercise.TaskItems.Add(new List<Concept>() { new Models.Space(15, 8) });
                exercise.TaskItems.Add(footerItems);
            }
        }

        protected override void BuildTaskItems()
        {
            // Configurate layout for FourPictures
            Lesson.HideRack = true;
            Lesson.TopMargins = new int[] { 20, 8 };
            Lesson.SingleChoice = true;

            base.BuildTaskItems();
        }
    }
}