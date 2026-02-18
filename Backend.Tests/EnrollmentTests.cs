using Backend.Domain.Entities;
using Xunit;

namespace Backend.Tests
{
    public class EnrollmentTests
    {
        [Fact]
        public void NewEnrollment_ShouldHaveRegisteredStatus()
        {
            var enrollment = new Enrollment(courseEventId: 1, participantId: 1);
            Assert.Equal(EnrollmentStatus.Registered, enrollment.Status);
        }

        [Fact]
        public void Cancel_ShouldSetStatusToCancelled()
        {
            var enrollment = new Enrollment(courseEventId: 1, participantId: 1);
            enrollment.Cancel();
            Assert.Equal(EnrollmentStatus.Cancelled, enrollment.Status);
        }
    }
}
