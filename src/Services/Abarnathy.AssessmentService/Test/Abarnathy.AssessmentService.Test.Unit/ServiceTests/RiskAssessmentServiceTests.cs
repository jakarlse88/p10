using Abarnathy.AssessmentService.Models;
using Abarnathy.AssessmentService.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Abarnathy.AssessmentService.Test.Unit.ServiceTests
{
    public class RiskAssessmentServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly PatientModel[] _patientsOver30;
        private readonly PatientModel[] _patientsUnder30;
        private readonly NoteModel[] _notesOver30;
        private readonly NoteModel[] _notesUnder30;

        public RiskAssessmentServiceTests()
        {
            _patientsOver30 = new[]
            {
                new PatientModel
                {
                    Id = 1,
                    DateOfBirth = new DateTime(1968, 06, 22),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 2,
                    DateOfBirth = new DateTime(1952, 09, 27),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 3,
                    DateOfBirth = new DateTime(1952, 11, 11),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 4,
                    DateOfBirth = new DateTime(1946, 11, 26),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 5,
                    DateOfBirth = new DateTime(1958, 06, 29),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 6,
                    DateOfBirth = new DateTime(1949, 12, 07),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 7,
                    DateOfBirth = new DateTime(1966, 12, 31),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 8,
                    DateOfBirth = new DateTime(1945, 06, 24),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 9,
                    DateOfBirth = new DateTime(1964, 06, 18),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 10,
                    DateOfBirth = new DateTime(1959, 06, 28),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 11,
                    DateOfBirth = new DateTime(1959, 06, 28),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 12,
                    DateOfBirth = new DateTime(1956, 06, 28),
                    SexId = 2
                }
            };

            _patientsUnder30 = new[]
            {
                new PatientModel
                {
                    Id = 1,
                    DateOfBirth = new DateTime(2010, 06, 22),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 2,
                    DateOfBirth = new DateTime(2010, 09, 27),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 3,
                    DateOfBirth = new DateTime(2010, 11, 11),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 4,
                    DateOfBirth = new DateTime(2010, 11, 26),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 5,
                    DateOfBirth = new DateTime(2010, 06, 29),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 6,
                    DateOfBirth = new DateTime(2010, 12, 07),
                    SexId = 2
                },
                new PatientModel
                {
                    Id = 7,
                    DateOfBirth = new DateTime(2010, 12, 31),
                    SexId = 1
                },
                new PatientModel
                {
                    Id = 8,
                    DateOfBirth = new DateTime(2010, 06, 24),
                    SexId = 2
                },
            };
            
            _notesOver30 = new []
            {
                new NoteModel
                {
                    PatientId = 1,
                    Content = "Patient states that they are \"feeling terrific\" " +
                              "Weight at or below recommended level"
                },
                new NoteModel
                {
                    PatientId = 1,
                    Content = "Patient states that they feel tired during the day" +
                              "Patient also complains about muscle aches" +
                              "Lab reports Microalbumin elevated"
                },
                new NoteModel
                {
                    PatientId = 1,
                    Content = "Patient states that they not feeling as tired" +
                              "Smoker, quit within last year" +
                              "Lab results indicate Antibodies present elevated"
                },
                new NoteModel
                {
                    PatientId = 2,
                    Content = "Patient states that they are feeling a great deal of stress at work" +
                              "Patient also complains that their hearing seems Abnormal as of late"
                },
                new NoteModel
                {
                    PatientId = 2,
                    Content = "Patient states that they have had a Reaction to medication within last 3 months" +
                              "Patient also complains that their hearing continues to be Abnormal"
                },
                new NoteModel
                {
                    PatientId = 2,
                    Content = "Lab reports Microalbumin elevated"
                },
                new NoteModel
                {
                    PatientId = 2,
                    Content = "Patient states that everything seems fine" +
                              "Lab reports Hemoglobin A1C above recommended level" +
                              "Patient admits to being long term Smoker"
                },
                new NoteModel
                {
                    PatientId = 3,
                    Content = "Patient states that they are short term Smoker"
                },
                new NoteModel
                {
                    PatientId = 3,
                    Content = "Lab reports Microalbumin elevated"
                },
                new NoteModel
                {
                    PatientId = 3,
                    Content = "Patient states that they are a Smoker, quit within last year" +
                              "Patient also complains that of Abnormal breathing spells" +
                              "Lab reports Cholesterol LDL high"
                },
                new NoteModel
                {
                    PatientId = 3,
                    Content = "Lab reports Cholesterol LDL high"
                },
                new NoteModel
                {
                    PatientId = 4,
                    Content = "Patient states that walking up stairs has become difficult" +
                              "Patient also complains that they are having shortness of breath" +
                              "Lab results indicate Antibodies present elevated" +
                              "Reaction to medication"
                },
                new NoteModel
                {
                    PatientId = 4,
                    Content = "Patient states that they are experiencing back pain when seated for a long time"
                },
                new NoteModel
                {
                    PatientId = 4,
                    Content = "Patient states that they are a short term Smoker" +
                              "Hemoglobin A1C above recommended level"
                },
                new NoteModel
                {
                    PatientId = 5,
                    Content = "Patient states that they experiencing occasional neck pain" +
                              "Patient also complains that certain foods now taste different" +
                              "Apparent Reaction to medication" +
                              "Body Weight above recommended level"
                },
                new NoteModel
                {
                    PatientId = 5,
                    Content = "Patient states that they had multiple dizziness episodes since last visit" +
                              "Body Height within concerned level"
                },
                new NoteModel
                {
                    PatientId = 5,
                    Content = "Patient states that they are still experiencing occaisional neck pain" +
                              "Lab reports Microalbumin elevated" +
                              "Smoker, quit within last year"
                },
                new NoteModel
                {
                    PatientId = 5,
                    Content = "Patient states that they had multiple dizziness episodes since last visit" +
                              "Lab results indicate Antibodies present elevated"
                },
                new NoteModel
                {
                    PatientId = 6,
                    Content = "Patient states that they feel fine" +
                              "Body Weight above recommended level"
                },
                new NoteModel
                {
                    PatientId = 6,
                    Content = "Patient states that they feel fine"
                },
                new NoteModel
                {
                    PatientId = 7,
                    Content = "Patient states that they often wake with stiffness in joints" +
                              "Patient also complains that they are having difficulty sleeping" +
                              "Body Weight above recommended level" +
                              "Lab reports Cholesterol LDL high"
                },
                new NoteModel
                {
                    PatientId = 8,
                    Content = "Lab results indicate Antibodies present elevated" +
                              "Hemoglobin A1C above recommended level"
                },
                new NoteModel
                {
                    PatientId = 9,
                    Content = "Patient states that they are having trouble concentrating on school assignments" +
                              "Hemoglobin A1C above recommended level"
                },
                new NoteModel
                {
                    PatientId = 9,
                    Content = "Patient states that they frustrated as long wait times" +
                              "Patient also complains that food in the vending machine is sub-par" +
                              "Lab reports Abnormal blood cell levels"
                },
                new NoteModel
                {
                    PatientId = 9,
                    Content = "Patient states that they are easily irritated at minor things" +
                              "Patient also complains that neighbors vacuuming is too loud" +
                              "Lab results indicate Antibodies present elevated"
                },
                new NoteModel
                {
                    PatientId = 10,
                    Content = "Patient states that they are not experiencing any problems"
                },
                new NoteModel
                {
                    PatientId = 10,
                    Content = "Patient states that they are not experiencing any problems" +
                              "Body Height within concerned level" +
                              "Hemoglobin A1C above recommended level"
                },
                new NoteModel
                {
                    PatientId = 10,
                    Content = "Patient states that they are not experiencing any problems" +
                              "Body Weight above recommended level" +
                              "Patient reports multiple dizziness episodes since last visit"
                },
                new NoteModel
                {
                    PatientId = 10,
                    Content = "Patient states that they are not experiencing any problems" +
                              "Lab reports Microalbumin elevated"
                },
                new NoteModel
                {
                    PatientId = 11,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"+
                              "Body  Weight"+
                              "Smoker "+
                              "Abnormal "+
                              "Cholesterol "+ 
                              "Dizziness "+
                              "Relapse "+
                              "Reaction "+
                              "Antibodies " 
                },
                new NoteModel
                {
                    PatientId = 12,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"+
                              "Body  Weight"+
                              "Smoker "+
                              "Abnormal "+
                              "Cholesterol "+ 
                              "Dizziness "+
                              "Relapse "+
                              "Reaction "+
                              "Antibodies " 
                }
            };

            _notesUnder30 = new[]
            {
                new NoteModel
                {
                    PatientId = 1,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"+
                              "Body  Weight"+
                              "Smoker "
                },
                new NoteModel
                {
                    PatientId = 2,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"+
                              "Body  Weight"+
                              "Smoker "+
                              "Abnormal "+
                              "Cholesterol "
                },
                new NoteModel
                {
                    PatientId = 3,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"
                },
                new NoteModel
                {
                    PatientId = 4,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "+
                              "Body  Height"+
                              "Body  Weight"
                },
                new NoteModel
                {
                    PatientId = 5,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "
                },
                new NoteModel
                {
                    PatientId = 6,
                    Content = "Hemoglobin A1C " +
                              "Microalbumin "
                },
                new NoteModel
                {
                    PatientId = 7,
                    Content = "Hemoglobin A1C " 
                },
                new NoteModel
                {
                    PatientId = 8,
                    Content = "Microalbumin "
                },
            };
            
            var projectPath =
                AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"obj\" }, StringSplitOptions.None)[0];
            _configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("TriggerTerms.json")
                .Build();
        }

        [Theory]
        [InlineData(1, RiskLevel.Borderline)]
        [InlineData(2, RiskLevel.Borderline)]
        [InlineData(3, RiskLevel.Borderline)]
        [InlineData(4, RiskLevel.Borderline)]
        [InlineData(5, RiskLevel.InDanger)]
        [InlineData(6, RiskLevel.None)]
        [InlineData(7, RiskLevel.Borderline)]
        [InlineData(8, RiskLevel.Borderline)]
        [InlineData(9, RiskLevel.Borderline)]
        [InlineData(10, RiskLevel.Borderline)]
        [InlineData(11, RiskLevel.EarlyOnset)]
        [InlineData(12, RiskLevel.EarlyOnset)]
        public async Task TestGenerateAssessmentPatientOver30(int patientId, RiskLevel expectedResult)
        {
            // Arrange
            var patientNotes = _notesOver30.Where(n => n.PatientId == patientId);

            var mockHistoryService = new Mock<IExternalHistoryAPIService>();
            mockHistoryService
                .Setup(x => x.GetPatientHistoryAsync(It.IsAny<int>()))
                .ReturnsAsync(patientNotes);

            var service = new RiskAssessmentService(mockHistoryService.Object, _configuration);

            // Act
            var result = await service.GenerateAssessment(_patientsOver30[patientId - 1]);

            // Assert
            Assert.Equal(expectedResult, result.RiskLevel);
        }
        
        [Theory]
        [InlineData(1, RiskLevel.EarlyOnset)]
        [InlineData(2, RiskLevel.EarlyOnset)]
        [InlineData(3, RiskLevel.InDanger)]
        [InlineData(4, RiskLevel.InDanger)]
        [InlineData(5, RiskLevel.None)]
        [InlineData(6, RiskLevel.None)]
        [InlineData(7, RiskLevel.None)]
        [InlineData(8, RiskLevel.None)]
        public async Task TestGenerateAssessmentPatientUnder30(int patientId, RiskLevel expectedResult)
        {
            // Arrange
            var patientNotes = _notesUnder30.Where(n => n.PatientId == patientId);

            var mockHistoryService = new Mock<IExternalHistoryAPIService>();
            mockHistoryService
                .Setup(x => x.GetPatientHistoryAsync(It.IsAny<int>()))
                .ReturnsAsync(patientNotes);

            var service = new RiskAssessmentService(mockHistoryService.Object, _configuration);

            // Act
            var result = await service.GenerateAssessment(_patientsUnder30[patientId - 1]);

            // Assert

            Assert.Equal(expectedResult, result.RiskLevel);
        }

        [Fact]
        public async Task TestGenerateAssessmentPatientNull()
        {
            // Arrange
            var service = new RiskAssessmentService(null, null);
            
            // Act
            async Task<AssessmentResult> TestAction() => await service.GenerateAssessment(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("patient", ex.ParamName);
        }

    }
}