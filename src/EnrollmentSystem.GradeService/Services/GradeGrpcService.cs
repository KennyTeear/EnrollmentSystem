using EnrollmentSystem.GradeService.Data;
using EnrollmentSystem.GradeService.Grpc;
using EnrollmentSystem.GradeService.Models;
using EnrollmentSystem.Shared.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollmentSystem.GradeService.Services;

public class GradeGrpcService : GradeGrpc.GradeGrpcBase
{
    private readonly GradeDbContext _db;

    public GradeGrpcService(GradeDbContext db)
    {
        _db = db;
    }

    public override async Task<GradeListReply> GetStudentGrades(StudentGradesRequest request, ServerCallContext context)
    {
        var grades = await _db.Grades.Where(g => g.StudentId == request.StudentId).ToListAsync();
        var reply = new GradeListReply();
        reply.Grades.AddRange(grades.Select(g => new GradeReply
        {
            Id = g.Id,
            StudentId = g.StudentId,
            CourseId = g.CourseId,
            CourseName = g.CourseName,
            CourseCode = g.CourseCode,
            NumericGrade = (double)g.NumericGrade,
            LetterGrade = g.LetterGrade,
            Semester = g.Semester,
            DatePosted = g.DatePosted.ToString("o")
        }));
        return reply;
    }

    public override async Task<GradeListReply> GetCourseGrades(CourseGradesRequest request, ServerCallContext context)
    {
        var grades = await _db.Grades.Where(g => g.CourseId == request.CourseId).ToListAsync();
        var reply = new GradeListReply();
        reply.Grades.AddRange(grades.Select(g => new GradeReply
        {
            Id = g.Id,
            StudentId = g.StudentId,
            CourseId = g.CourseId,
            CourseName = g.CourseName,
            CourseCode = g.CourseCode,
            NumericGrade = (double)g.NumericGrade,
            LetterGrade = g.LetterGrade,
            Semester = g.Semester,
            DatePosted = g.DatePosted.ToString("o")
        }));
        return reply;
    }

    public override async Task<UploadGradeReply> UploadGrade(EnrollmentSystem.GradeService.Grpc.UploadGradeRequest request, ServerCallContext context)
    {
        var grade = new Grade
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            NumericGrade = (decimal)request.NumericGrade,
            LetterGrade = request.LetterGrade,
            Semester = "",
            DatePosted = DateTime.UtcNow
        };
        _db.Grades.Add(grade);
        await _db.SaveChangesAsync();
        return new UploadGradeReply { Success = true };
    }
}