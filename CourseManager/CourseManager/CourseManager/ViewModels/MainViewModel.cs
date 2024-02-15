using Caliburn.Micro;
using CourseManager.Models;
using CourseManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.ViewModels
{
    class MainViewModel : Screen
    {
        private BindableCollection<EnrollmentModel> _enrollments = new BindableCollection<EnrollmentModel>();
        private BindableCollection<StudentModel> _students = new BindableCollection<StudentModel>();
        private BindableCollection<CourseModel> _courses = new BindableCollection<CourseModel>();
        private readonly string _connectionString = @"Data Source=AZITD455;Initial Catalog=CourseReport;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private string _appStatus;
        private EnrollmentModel _selectedEnrollment;
        private EnrollmentCommand _enrollmentCommand;
        public MainViewModel()
        {
            SelectedEnrollment  = new EnrollmentModel();
            try
            {
                _enrollmentCommand = new EnrollmentCommand(_connectionString);
                Enrollments.AddRange(_enrollmentCommand.GetList());

                StudentsCommand studentCommand = new StudentsCommand(_connectionString);
                Students.AddRange(studentCommand.GetList());

                CourseCommand courseCommand = new CourseCommand(_connectionString);
                Courses.AddRange(courseCommand.GetList());
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);

            }
        }
        public CourseModel SelectedEnrollmentCourse
        {
            get
            {
                try
                {
                    var courseDictionary = _courses.ToDictionary(b => b.CourseId);

                    if (SelectedEnrollment != null && courseDictionary.ContainsKey(SelectedEnrollment.CourseId))
                    {
                        return courseDictionary[SelectedEnrollment.CourseId];
                    }
                }
                    catch (Exception ex)
                    {
                        UpdateAppStatus(ex.Message);
                    }

                return null;
            }

            set
            {
                try
                {
                    var selectedEnrollmentCourse = value;

                    SelectedEnrollment.CourseId = selectedEnrollmentCourse.CourseId;
                    
                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch(Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }


        public StudentModel SelectedEnrollmentStudent
        {
            get
            {
                try
                {
                    var studentDictionary = _students.ToDictionary(b => b.StudentId);

                    if (SelectedEnrollment != null && studentDictionary.ContainsKey(SelectedEnrollment.StudentId))
                    {
                        return studentDictionary[SelectedEnrollment.StudentId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentStudent = value;

                    SelectedEnrollment.StudentId = selectedEnrollmentStudent.StudentId;

                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        public BindableCollection<EnrollmentModel> Enrollments
        {
            get { return _enrollments; }
            set { _enrollments = value; }
        }

        public BindableCollection<StudentModel> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        public BindableCollection<CourseModel> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public string AppStatus
        {
            get { return _appStatus; }
            set { _appStatus = value; }
        }


        public EnrollmentModel SelectedEnrollment
        {
            get
            {
                return _selectedEnrollment;
            }
            set
            {
                _selectedEnrollment = value;
                NotifyOfPropertyChange(() => SelectedEnrollment);
                NotifyOfPropertyChange(() => SelectedEnrollmentCourse);
                NotifyOfPropertyChange(() => SelectedEnrollmentStudent);
            }
        }

        public void UpdateNewEnrollment()
        {
            try
            {
                SelectedEnrollment = new EnrollmentModel();
                AppStatus = "New enrollment created";
                NotifyOfPropertyChange(() => AppStatus);
                

            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }
        public void CreatenewEnrollment()
        {
            try
            {
                SelectedEnrollment = new EnrollmentModel();
                AppStatus = "New Enrollment Created";
                NotifyOfPropertyChange(() => AppStatus);
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }

        public void SaveEnrollment()
        {
            try
            {
                var enrollmentDicctionarry = _enrollments.ToDictionary(p => p.EnrollmentId);

                if(SelectedEnrollment != null)
                {
                    _enrollmentCommand.Upsert(SelectedEnrollment);

                    Enrollments.Clear();
                    Enrollments.AddRange(_enrollmentCommand.GetList());

                    AppStatus = "Enrollment saved";
                    NotifyOfPropertyChange(() => AppStatus);
                }
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }
        private void UpdateAppStatus(string message)
        {
            AppStatus = message;
            NotifyOfPropertyChange(() => AppStatus);
        }
    }
}
