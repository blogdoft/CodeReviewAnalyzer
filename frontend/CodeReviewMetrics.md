# Code review Metrics

Tracking **code review metrics** is essential to ensure software quality, improve the development process, and reduce failures in production. Below, I list the **key metrics**, how to measure them, and **acceptable targets**.

## **1. Average Review Time (Time to Review)**
### **What does it measure?**
The time from opening the Pull Request until the first review is performed by a reviewer.

### **How to measure it?**
- **Capture** the date/time of the PR creation.
- **Capture** the date/time of the first comment from a reviewer.

### **Formula:**
\[
Review \ Time = Date_{First\_Review} - Date_{PR\_Creation}
\]

### **Target:**
✅ **Less than 4 hours** for small PRs.  
✅ **Less than 8 hours** for medium PRs.  
✅ **Maximum of 24 hours** for large PRs.

If it exceeds 24 hours, it may indicate **workflow delays**.

---

## **2. Average Time to Approval (Time to Approve)**
### **What does it measure?**
How long it takes for a PR to be approved from the time it is created.

### **How to measure it?**
- **Capture** the date/time of the PR creation.
- **Capture** the date/time of the final approval.

### **Formula:**
\[
Time \ to \ Approval = Date_{Approval} - Date_{PR\_Creation}
\]

### **Target:**
✅ **Less than 24 hours** for small PRs.  
✅ **Less than 48 hours** for medium PRs.  
✅ **Maximum of 72 hours** for large PRs.

If it exceeds 72 hours, it may indicate **excessive bureaucracy** or **difficulty understanding the code**.

---

## **3. Average Time to Merge (Time to Merge)**
### **What does it measure?**
How long a PR takes from creation until it is merged into the main branch.

### **How to measure it?**
- **Capture** the date/time of the PR creation.
- **Capture** the date/time of the merge.

### **Formula:**
\[
Time \ to \ Merge = Date_{Merge} - Date_{PR\_Creation}
\]

### **Target:**
✅ **Less than 24 hours** for small PRs.  
✅ **Less than 72 hours** for medium PRs.  
✅ **Maximum of 5 days** for large PRs.

If it takes too long, it may indicate **lack of active reviewers** or **too many changes in the PR**.

---

## **4. Average Number of Iterations (Review Iterations per PR)**
### **What does it measure?**
The number of reviews and changes required before the PR is approved.

### **How to measure it?**
- Count how many times the PR was updated with changes after feedback.

### **Target:**
✅ **1 or 2 iterations** is ideal.  
⚠ **More than 3 iterations** may indicate poorly written code or communication issues between the team and reviewers.  

If a PR requires **many changes**, it may indicate **low initial code quality**.

---

## **5. Average Number of Comments per PR**
### **What does it measure?**
How many review comments are made in a PR.

### **How to measure it?**
- Count the number of comments made by reviewers.

### **Target:**
✅ **3 to 10 comments** is a good number.  
⚠ **Less than 3 comments** may indicate **a superficial review**.  
❌ **More than 15 comments** may indicate **poor code quality**.  

If there are too many comments and revisions, it may be a **sign of unclear code or poor practices**.

---

## **6. Percentage of PRs with Post-Merge Bugs**
### **What does it measure?**
Whether a PR was **approved, merged, and later caused a bug**.

### **How to measure it?**
- Count how many PRs have **bugs linked to Work Items**.
- Divide by the total number of PRs.

### **Formula:**
\[
\% Bugs \ per \ PR = \left( \frac{PRs \ with \ Bugs}{Total \ PRs} \right) \times 100
\]

### **Target:**
✅ **Less than 5%** of PRs should generate bugs.  
⚠ **Between 5% and 10%** may indicate the need for more tests.  
❌ **Above 10%** suggests that the Code Review is failing and bugs are going unnoticed.  

If this number is high, it may be necessary to **improve automated testing and code review quality**.

---

## **7. Percentage of PRs Approved on First Attempt**
### **What does it measure?**
Whether a PR was **approved without requiring changes**.

### **How to measure it?**
- Count how many PRs were **approved on the first attempt**.
- Divide by the total number of PRs.

### **Formula:**
\[
\% Approved \ on \ First \ Try = \left( \frac{PRs \ approved \ without \ iterations}{Total \ PRs} \right) \times 100
\]

### **Target:**
✅ **40% to 60%** of PRs should be approved on the first try.  
⚠ **Below 40%** may indicate low initial code quality.  
❌ **Above 80%** may indicate that reviewers are not being critical enough.  

---

## **Summary of Metrics and Targets**
| Metric | How to Measure? | Target |
|---------|------------|--------|
| **Review Time** | First comment after creation | < 4h (small), < 24h (large) |
| **Time to Approval** | Last approval - Creation | < 24h (small), < 72h (large) |
| **Time to Merge** | Merge - Creation | < 5 days |
| **Number of Iterations** | Number of changes after feedback | 1-2 reviews |
| **Comments per PR** | Number of feedbacks | 3-10 |
| **% PRs with Bugs** | PRs with Bugs / Total PRs | < 5% |
| **% Approved on First Try** | PRs without changes / Total PRs | 40%-60% |
