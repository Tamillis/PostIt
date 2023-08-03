using PostItDemo.Controllers;
using PostItDemo.Models;
using System.Security.Claims;

namespace PostItsTests
{
    public class GivenUtils
    {
        [TestCase(0, 1)]
        [TestCase(1, 2)]
        public void GetPostValue_WithTodayAndGivenLikes_ValueIsExpected(int likes, double expected)
        {
            //Arrange
            var testPost = new PostIt() { AuthorLikes = new List<AuthorLike>(), Uploaded = DateTime.Now };
            for (int n = 0; n < likes; n++) testPost.AuthorLikes.Add(new AuthorLike());

            //Act
            var value = Utils.GetPostValue(testPost);

            //Assert, within enough precision to allow for the small diff in time between Arrange and Assert
            Assert.That(value, Is.EqualTo(expected).Within(0.000001));

        }

        [Test]
        public void HandleIsIllegal_WithLegalHandle_ReturnsFalse()
        {
            Assert.That(Utils.HandleIsIllegal("Tester"), Is.False);
        }

        [Test]
        public void HandleIsIllegal_WithIllegalHandle_ReturnsTrue()
        {
            Assert.That(Utils.HandleIsIllegal("Anon"), Is.True);
        }

        [Test]
        public void UserHasHandle_WithValidUser_ReturnsTrue()
        {
            //Arrange
            var claimsPrincipal = GetDummyPrincipal(WithHandleClaim: true);

            //Act
            var result = Utils.UserHasHandle(claimsPrincipal);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void UserHasHandle_WithoutValidUser_ReturnsFalse()
        {
            //Arrange
            var claimsPrincipal = GetDummyPrincipal();

            //Act
            var result = Utils.UserHasHandle(claimsPrincipal);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetUserHandle_WithValidUser_ReturnsHandle()
        {
            //Arrange
            var claimsPrincipal = GetDummyPrincipal(WithHandleClaim: true);

            //Act
            var result = Utils.GetUserHandle(claimsPrincipal);

            //Assert
            Assert.That(result, Is.EqualTo("Test"));
        }

        private ClaimsPrincipal GetDummyPrincipal(bool WithHandleClaim = false)
        {
            var claims = new List<Claim>();
            if (WithHandleClaim) claims.Add(new Claim("Handle", "Test"));
            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}