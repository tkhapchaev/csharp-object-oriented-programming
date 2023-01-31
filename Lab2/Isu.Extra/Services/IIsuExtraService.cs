using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Models;
using Stream = Isu.Extra.Entities.Stream;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
    MainGroup AddGroup(GroupName groupName);

    Student AddStudent(MainGroup group, string name);

    void ChangeStudentGroup(Student student, MainGroup newGroup);

    Ognp AddOgnp(string ognpName, FacultyName facultyName);

    Stream AddOgnpStream(Ognp ognp, Teacher teacher);

    OgnpGroup AddOgnpGroup(Stream stream);

    void EnrollStudentInOgnp(Student student, Ognp ognp);

    void DisenrollStudentFromOgnp(Student student);

    Ognp GetOgnp(string ognpName);

    List<Stream> GetOgnpStreams(string ognpName);

    List<Student> GetStudentsFromOgnpGroup(OgnpGroup ognpGroup);

    List<Student> GetNonEnrolledStudents();
}