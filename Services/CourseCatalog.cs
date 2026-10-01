using ApsCourseFinder.Models;
using static ApsCourseFinder.Models.Subjects;
using R = ApsCourseFinder.Models.Requirement;

namespace ApsCourseFinder.Services;

/// <summary>
/// All CUT programmes, taken from the CUT Subject Selection Guide 2023.
/// To add or change a course, edit the list below - nothing else needs to change.
/// </summary>
public class CourseCatalog
{
    private const int Pass = 30;
    private const string Bfn = "Bloemfontein";
    private const string Both = "Bloemfontein & Welkom";
    private const string NoMin = "The CUT guide lists these subjects but no minimum mark, so a pass (30%) is assumed. Please confirm with CUT.";
    private const string EngNote = "Mathematical Literacy is not accepted for engineering and built environment programmes.";

    public IReadOnlyList<Course> Courses { get; } = Build();

    private static Course C(Faculty f, string name, string field, int? aps, string duration, string campus,
        string careers, R[] reqs, string? note = null, int? allMin = null) => new()
    {
        Faculty = f, Name = name, Field = field, MinAps = aps, Duration = duration, Campus = campus,
        Careers = careers, Requirements = reqs, Note = note, AllSubjectsMin = allMin
    };

    private static List<Course> Build()
    {
        const Faculty H = Faculty.Health, E = Faculty.Engineering, M = Faculty.Management, U = Faculty.Humanities;

        // Reusable requirement sets
        R[] diploma = [R.One(Maths, 50), R.One(PhysSci, 50), R.One(English, 40)];
        R[] degree = [R.One(Maths, 60), R.One(PhysSci, 50), R.One(English, 50)];
        R[] construction = [R.One(English, 50), R.One(Maths, 50), R.One(PhysSci, 50)];
        R[] fourSubjects = [R.One(Maths, Pass), R.One(PhysSci, Pass), R.One(LifeSci, Pass), R.One(English, Pass)];
        R[] ict = [R.Mixed((Maths, 40), (MathLit, 60), (IT, 60)), R.One(English, 50)];
        R[] accountancy = [R.One(Accounting, 60), R.Mixed((Maths, 40), (MathLit, 60))];
        R[] accountancyEcp = [R.One(Accounting, 50), R.Mixed((Maths, 30), (MathLit, 50))];
        R[] none = [];
        const string bEd = "Teacher or lecturer at a secondary school or FET college.";
        const string bEdNote = "Needs an NSC with endorsement for admission to a bachelor's degree (matric exemption).";

        return new List<Course>
        {
            // ---------- Engineering, Built Environment & IT ----------
            C(E, "Diploma in Engineering Technology in Civil Engineering", "Civil Engineering", 27, "2 years", Bfn,
              "Technologist, technician or engineer in construction and building, consulting engineering, research and entrepreneurship.", diploma, EngNote),
            C(E, "Bachelor of Engineering Technology in Civil Engineering", "Civil Engineering", 32, "3 years", Bfn,
              "Technologist, technician or engineer in construction and building, consulting engineering, research and entrepreneurship.", degree, EngNote),
            C(E, "Higher Certificate in Construction", "Construction Management", 27, "1 year", Bfn,
              "Construction management and site roles.", construction, "Mathematical Literacy is not accepted. The guide gives one set of subject rules for this whole field of study."),
            C(E, "Bachelor of Construction in Construction Management", "Construction Management", 32, "3 years", Bfn,
              "Construction Manager.", construction, "Mathematical Literacy is not accepted."),
            C(E, "Bachelor of Construction in Health and Safety Management", "Construction Management", 32, "3 years", Bfn,
              "Health and safety management in the construction industry.", construction, "Mathematical Literacy is not accepted."),
            C(E, "Diploma in Engineering Technology in Electrical Engineering", "Electrical Engineering", 27, "2 years", Bfn,
              "Technologist or technician: automation, mining and transportation, medical electronics, sound and stage, programming.", diploma, EngNote),
            C(E, "Bachelor of Engineering Technology in Electrical Engineering", "Electrical Engineering", 32, "3 years", Bfn,
              "Technologist or technician: automation, mining and transportation, medical electronics, sound and stage, programming.", degree, EngNote),
            C(E, "Diploma in Engineering Technology in Mechanical Engineering", "Mechanical Engineering", 27, "2 years", Bfn,
              "Maintenance in mining, steel, petrochemical, manufacturing and power generation; project engineering; design and testing of mechanical devices.", diploma, EngNote),
            C(E, "Bachelor of Engineering Technology in Mechanical Engineering", "Mechanical Engineering", 32, "3 years", Bfn,
              "Maintenance in mining, steel, petrochemical, manufacturing and power generation; project engineering; design and testing of mechanical devices.", degree, EngNote),
            C(E, "Bachelor of Construction in Quantity Surveying", "Quantity Surveying", 32, "3 years", Bfn,
              "Quantity Surveyor.", construction, "Mathematical Literacy is not accepted."),
            C(E, "Higher Certificate in Renewable Energy Technologies", "Renewable Energy", 27, "1 year", Bfn,
              "Technician installing, commissioning and maintaining renewable energy systems.", diploma, EngNote),
            C(E, "Higher Certificate in Information Technology", "Information and Communication Technologies", 27, "1 year", Both,
              "IT network technician, network and infrastructure roles, software and web development support.", ict),
            C(E, "Diploma in Computer Networking", "Information and Communication Technologies", 27, "3 years", Bfn,
              "IT network technician, network engineer, network infrastructure analyst.", ict),
            C(E, "Diploma in Information Technology", "Information and Communication Technologies", 27, "3 years", Both,
              "Software, web, mobile app and database analyst/developer; systems analyst; IT project leader.", ict),
            C(E, "Bachelor of Science in Hydrology and Water Resources Management", "Hydrology & Water Resources", 28, "3 years", Bfn,
              "Consulting engineering and environmental industry, mining industry, research.",
              [R.One(Maths, 50), R.One(PhysSci, 50), R.One(LifeSci, 50), R.One(English, 50)],
              "The guide lists \"Life Sciences/Physiology\"; only Life Sciences is offered here."),
            C(E, "Higher Certificate in Mathematics for Engineering Technology", "Mathematics for Engineering Technology", null, "1 year", Bfn,
              "Access to STEM-related qualifications at universities, including engineering and engineering technology.",
              [R.One(English, 40), R.Any(50, PhysSci, TechSci), R.Mixed((Maths, 30), (TechMaths, 30), (MathLit, 60))],
              "No APS is listed for this programme. Entry is by subject levels only. If you use Mathematical Literacy, you also need 40% in Grade 11 Mathematics."),

            // ---------- Health & Environmental Science ----------
            C(H, "Clinical Technology", "Clinical Technology", 30, "4 years", Bfn,
              "Clinical Technologist in government or private practice: cardiology, pulmonology, critical care, nephrology, reproductive biology, perfusion, neurophysiology.", fourSubjects, NoMin),
            C(H, "Agricultural Management", "Agricultural Management", 27, "3 years", Bfn,
              "Farm manager, agricultural scientist, research technician, banking, seed and fertiliser companies, feedlot manager, own business.",
              [R.AtLeast(2, Pass, AgriSci, LifeSci, PhysSci, Maths, MathLit, Accounting, Economics)], NoMin),
            C(H, "Somatology (incl. Therapeutic Services)", "Somatology", 27, "3 years", Bfn,
              "Beauty or spa therapist, cruise-ship therapist, spa manager, health and beauty sales, researcher or lecturer.",
              [R.Any(Pass, PhysSci, LifeSci)], NoMin),
            C(H, "Medical Laboratory Sciences", "Medical Laboratory Sciences", 30, "4 years", Bfn,
              "Clinical analysis (chemistry, immunology, haematology, microbiology) and public health (forensics, epidemiology).", fourSubjects, NoMin),
            C(H, "Dental Assistant", "Dental Assistant", null, "Not stated", Bfn,
              "Dental assistance in public and private practice.",
              [R.Any(Pass, LifeSci, PhysSci)], "The guide lists no APS or duration for this programme. " + NoMin),
            C(H, "Environmental Health", "Environmental Health", 30, "4 years", Bfn,
              "Environmental health practitioner, meat inspector, waste management officer, occupational health and safety, port health.", fourSubjects, NoMin),
            C(H, "Radiography in Diagnostics", "Radiography in Diagnostics", 30, "4 years", Bfn,
              "Radiographer in diagnostic, therapy, nuclear medicine and ultrasound; lecturer or researcher.", fourSubjects, NoMin),

            // ---------- Management Sciences ----------
            C(M, "Bachelor of Management Sciences in Accountancy", "Accountancy", 27, "4 years", Both,
              "Professional accountant in commerce, industry, academia and the public sector; pathway to the SAIA/IIA internal audit programmes.", accountancy,
              "Rules shown are for NSC (2008 onwards)."),
            C(M, "ECP in Accountancy", "Accountancy", 27, "1 year plus 4 years of mainstream", Both,
              "Extended programme leading to the Accountancy degree.", accountancyEcp, "Rules shown are for NSC (2008 onwards)."),
            C(M, "Bachelor of Management Sciences in Internal Auditing", "Internal Auditing", 27, "4 years", Both,
              "Internal auditor; access to Internal Audit Technician and Professional Internal Auditor programmes.", accountancy,
              "Rules shown are for NSC (2008 onwards)."),
            C(M, "ECP in Internal Auditing", "Internal Auditing", 27, "1 year plus 4 years of mainstream", Both,
              "Extended programme leading to the Internal Auditing degree.", accountancyEcp, "Rules shown are for NSC (2008 onwards)."),
            C(M, "Hospitality Management", "Hospitality Management", 27, "3 years", Bfn,
              "Hotel manager, front-office manager, banqueting or food and beverage manager, restaurateur, caterer, entrepreneur.", none),
            C(M, "ECP in Hospitality", "Hospitality Management", 23, "Extended programme", Bfn,
              "Extended programme leading to Hospitality Management.", none),
            C(M, "Human Resources Management", "Human Resources Management", 27, "3 years", Both,
              "Human resources development practitioner, employment and labour relations specialist, HRM or HRD specialist.", none),
            C(M, "Marketing", "Marketing", 27, "3 years", Both,
              "Marketer or salesperson, advertising and promotion manager, marketing consultant, sales manager, export agent.", none),
            C(M, "Office Management and Technology", "Office Management and Technology", 27, "3 years", Both,
              "Secretary, data processor, administrative or personal assistant, legal secretary, personnel officer.", none),
            C(M, "Public Management", "Public Management", 27, "3 years", Both,
              "Public sector positions in national, regional and local government: finance and procurement, HR, office and project management.", none),
            C(M, "Tourism Management", "Tourism Management", 28, "3 years", Bfn,
              "Tour operator, travel agent, airline industry, tourism marketer, event and conference organiser, car-rental agent, tourism official.", none),
            C(M, "Community Development Work", "Community Development Work", 26, "1 year full-time or 2 years part-time", Bfn,
              "Community development worker in government, NGOs and local development initiatives.", none,
              "Candidates below 26 points may still be considered through a selection process."),

            // ---------- Humanities ----------
            C(U, "Languages Practice and Media Studies", "Languages Practice and Media Studies", 27, "3 years", Both,
              "Language officer, translator, communications officer, training officer, journalist, public relations officer.",
              [R.AtLeast(2, 60, Languages) with { Label = "60% in two official languages (your home language and first additional language)" }]),
            C(U, "Design and Studio Art", "Design and Studio Art", 27, "3 years", Bfn,
              "Fashion, advertising, communications, packaging, jewellery, illustration and multi-media visual arts.", none,
              "You must also submit a prescribed portfolio and complete a selection test and interview. Art & Design, Visual Art or Design Theory are recommended.", allMin: 50),
            C(U, "B.Ed (SP & FET): Computer Science", "B.Ed Computer Science", 27, "4 years", Both, bEd,
              [R.One(Maths, 50)], bEdNote + " Computer Applications Technology (CAT) is recommended."),
            C(U, "B.Ed (SP & FET): Economic & Management Science", "B.Ed Economic & Management Science", 27, "4 years", Both, bEd,
              [R.One(Accounting, 50), R.Any(50, Economics, BusStudies)], bEdNote),
            C(U, "B.Ed Foundation Phase Teaching", "B.Ed Foundation Phase", 27, "4 years", Both,
              "Qualified educator in primary schools.", [R.One(Maths, 50)], bEdNote),
            C(U, "B.Ed (SP & FET): Language Education", "B.Ed Language Education", 27, "4 years", Both, bEd,
              [R.One(English, 50), R.Any(50, Languages.Where(l => l != English).ToArray()) with { Label = "50% in one more official language (besides English)" }], bEdNote),
            C(U, "B.Ed (SP & FET): Mathematics", "B.Ed Mathematics", 27, "4 years", Both, bEd,
              [R.One(Maths, 50), R.One(PhysSci, 50)], bEdNote),
            C(U, "B.Ed (SP & FET): Natural Science", "B.Ed Natural Science", 27, "4 years", Both, bEd,
              [R.One(Maths, 50), R.One(PhysSci, 50), R.One(LifeSci, 50)], bEdNote),
            C(U, "B.Ed (SP & FET): Technology", "B.Ed Technology", 27, "4 years", Bfn, bEd,
              [R.Any(50, Maths, EGD)], bEdNote + " Technical Drawing or Engineering Graphics and Design is recommended."),
        };
    }
}
