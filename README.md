
# EmployAI

**EmployAI** is my latest big project, an AI-powered assistant designed to help streamline the hiring process. It features a resume parser, a ranking system to evaluate candidates, and an AI chatbot to answer hiring questions. Pretty cool, right?

> ⚠️ **Just a heads up**: While this app is pretty awesome, the data you see on the site is for demo purposes. It's not 100% accurate, so use it with a pinch of salt!

---

## 🧠 What It Can Do

- **Resume Parsing**: Extracts and analyzes candidate information from resumes in various formats (PDF, DOCX, Images, etc.).
- **Candidate Ranking**: Rates candidates based on their experience, education, skills, and other factors.
- **User Registration & Login**: Users can register and log in to save progress and upload resumes.
- **Feedback System**: Allows users to leave feedback on the system for future improvements.
- **Database Storage**: All data, including resumes and user details, are stored securely in a database for easy retrieval and management.
- **Unique Search Feature**: Search for anything within the resumes or database — a powerful, flexible search engine.
- **OCR (Optical Character Recognition)**: Extracts text from image-based resumes or documents, turning them into searchable and analyzable content.
- **Dashboard**: A user-friendly interface that gives quick access to the various employee filtering functionalities.


---

## 🛠️ Tech I Used

- **Frontend**: Built using .NET (C#), HTML, CSS, and JavaScript
- **Backend**: Powered by Python
- **AI Models**: For parsing resumes and powering the chatbot

---

## 🚀 How to Run It

### Prerequisites

- Install **.NET SDK**: [Download here](https://dotnet.microsoft.com/download)
- Install **Python 3.x**: [Download here](https://www.python.org/downloads/)
- Install **pip**: [Follow this guide](https://pip.pypa.io/en/stable/installation/)

### Steps to Get It Up and Running

1. **Clone the repo:**

   ```bash
   git clone https://github.com/Outlerjjr/EmployAI.git
   cd EmployAI
   ```
   
2. **Set up the database:""

Open SQL Server Management Studio (SSMS).

Create a new Database or restore the .bak file from the repo to set up the database.

Ensure that you configure the connection strings in the backend to point to the correct SQL Server instance.
3. **Set up the backend:**

   ```bash
   cd Backend_py
   pip install -r requirements.txt
   ```

4. **Run the backend:**

   ```bash
   python run.py
   ```

5. **Set up the frontend:**

   Open `Frontend_net` in your favorite IDE (like Visual Studio) and build the project.

6. **Open it in your browser**:  
   Head to `http://localhost:5004` — and voilà, you’re good to go!

---

## 📌 Disclaimer

Just a quick note: this app is here to show you how an AI-based hiring assistant might work, but keep in mind, the data you see isn't perfectly accurate. So, don’t rely solely on it for major decisions!

---

## 🚨 Important Notice

This is my biggest claim to fame yet, and I’m planning to work on some even bigger projects in the future. I might update this one and add some cool new features soon, so stay tuned!

---

## 📄 License

This project is under the MIT License.

---
