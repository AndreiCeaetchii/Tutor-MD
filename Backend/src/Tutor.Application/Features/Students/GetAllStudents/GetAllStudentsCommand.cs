using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Students.DTOs;

namespace Tutor.Application.Features.Students.GetAllStudents;

public record GetAllStudentsCommand() :  IRequest<Result<List<StudentDto>>>;
