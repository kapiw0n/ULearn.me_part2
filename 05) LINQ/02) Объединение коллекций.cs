public static string[] GetAllStudents(Classroom[] classes)
{
    return classes
        .SelectMany(x => x.Students)
        .ToArray();
}