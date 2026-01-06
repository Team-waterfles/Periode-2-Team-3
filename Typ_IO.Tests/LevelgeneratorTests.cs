using Typ_IO.Core.Services;
using Xunit;

namespace Typ_IO.Tests
{
    public class LevelgeneratorTests
    {
        [Fact]
        public void BevatAlleenToegestaneKarakters_MetGeldigeTekst_ReturnsTrue()
        {
            // Arrange
            string tekst = "fjdk";
            string toegestaneKarakters = "fjdksl";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.True(resultaat);
        }

        [Fact]
        public void BevatAlleenToegestaneKarakters_MetOngeldigeTekst_ReturnsFalse()
        {
            // Arrange
            string tekst = "fxyz";
            string toegestaneKarakters = "fjdk";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.False(resultaat);
        }

        [Fact]
        public void BevatAlleenToegestaneKarakters_MetLegeTekst_ReturnsTrue()
        {
            // Arrange
            string tekst = "";
            string toegestaneKarakters = "fjdk";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.True(resultaat);
        }

        [Fact]
        public void BevatAlleenToegestaneKarakters_MetNullTekst_ReturnsTrue()
        {
            // Arrange
            string tekst = null;
            string toegestaneKarakters = "fjdk";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.True(resultaat);
        }

        [Fact]
        public void BevatAlleenToegestaneKarakters_MetLegeToegstaneKarakters_ReturnsFalse()
        {
            // Arrange
            string tekst = "fjdk";
            string toegestaneKarakters = "";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.False(resultaat);
        }

        [Fact]
        public void BevatAlleenToegestaneKarakters_MetSpaties_ReturnsTrue()
        {
            // Arrange
            string tekst = "f j d k";
            string toegestaneKarakters = " fjdk";

            // Act
            bool resultaat = Levelgenerator.BevatAlleenToegestaneKarakters(tekst, toegestaneKarakters);

            // Assert
            Assert.True(resultaat);
        }
    }
}