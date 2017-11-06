using System.Linq;
using Xunit;

namespace MicroKnights.Collections.Test
{
    public class TestRoleTypeEnumeration
    {
        [Fact]
        public void GetAll()
        {
            var all = UserRoleType.GetAll();

            Assert.True(all.Length == 4);
        }

        [Fact]
        public void Test()
        {
            var roleTypeValueFromDatabase = 10;
            var roleType = UserRoleType.FromValueOrDefault(roleTypeValueFromDatabase, UserRoleType.Unknown);

            if (roleType.CanCreateUser)
            {
                // create user..
            }

            // Find roles with specific rights
            var rolesThatCanResetPassword = UserRoleType.GetAll().Where(urt => urt.CanResetUserPassword);
        }
    }
}