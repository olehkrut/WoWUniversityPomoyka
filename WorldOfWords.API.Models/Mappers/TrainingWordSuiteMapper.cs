﻿using System.Collections.Generic;
using System.Linq;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public class TrainingWordSuiteMapper:ITrainingWordSuiteMapper
    {
        ITrainingWordTranslationMapper _mapper;

        public TrainingWordSuiteMapper(ITrainingWordTranslationMapper mapper)
        {
            this._mapper = mapper;
        }

        public TrainingWordSuiteModel Map(WordSuite WordSuite)
        {
            TrainingWordSuiteModel wordSuiteModel = new TrainingWordSuiteModel();
            wordSuiteModel.Id = WordSuite.Id;
            wordSuiteModel.OwnerId = WordSuite.OwnerId;
            wordSuiteModel.QuizResponseTime = WordSuite.QuizResponseTime;
            wordSuiteModel.QuizStartTime = WordSuite.QuizStartTime;
            wordSuiteModel.Name = WordSuite.Name;
            wordSuiteModel.LanguageId = WordSuite.LanguageId;
            wordSuiteModel.Threshold = WordSuite.Threshold;
            wordSuiteModel.WordTranslations = new List<WordTranslationModel>();
            wordSuiteModel.WordTranslations.AddRange(WordSuite.WordProgresses.Select(x => _mapper.Map(x.WordTranslation)).ToList());
            for (int i = 0; i < wordSuiteModel.WordTranslations.Count; i++)
            {
                wordSuiteModel.WordTranslations[i].Progress = (int)(WordSuite.WordProgresses.ToList())[i].Progress;

            }
            return wordSuiteModel;
        }

        public TrainingWordSuiteModel MapSpecial(WordSuite WordSuite)
        {
            var allowedMistakes = 2;

            TrainingWordSuiteModel wordSuiteModel = new TrainingWordSuiteModel();
            wordSuiteModel.Id = WordSuite.Id;
            wordSuiteModel.OwnerId = WordSuite.OwnerId;
            wordSuiteModel.QuizResponseTime = WordSuite.QuizResponseTime;
            wordSuiteModel.QuizStartTime = WordSuite.QuizStartTime;
            wordSuiteModel.Name = WordSuite.Name;
            wordSuiteModel.LanguageId = WordSuite.LanguageId;
            wordSuiteModel.Threshold = WordSuite.Threshold;
            wordSuiteModel.WordTranslations = new List<WordTranslationModel>();

            var badWordProgress = WordSuite.WordProgresses.Where(wp => wp.NumOfMistakes >= allowedMistakes).ToList();

            wordSuiteModel.WordTranslations.AddRange(WordSuite.WordProgresses
                .Where(wp => wp.NumOfMistakes >= allowedMistakes).Select(x => _mapper.Map(x.WordTranslation)).ToList());


            for (int i = 0; i < wordSuiteModel.WordTranslations.Count; i++)
            {
                var wordProgress = badWordProgress[i];

                wordSuiteModel.WordTranslations[i].Progress = (int)wordProgress.Progress;
                wordSuiteModel.WordTranslations[i].NumOfMistakes = wordProgress.NumOfMistakes;
            }
            return wordSuiteModel;
        }

        public List<TrainingWordSuiteModel> Map(List<WordSuite> WordSuites)
        {
            return WordSuites.Select(x => Map(x)).ToList(); ;
        }

        public List<TrainingWordSuiteModel> MapSpecial(List<WordSuite> WordSuites)
        {
            return WordSuites.Select(x => MapSpecial(x)).ToList(); ;
        }
    }
}
