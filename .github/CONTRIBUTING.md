# Contributing to AutoEntityGenerator

Thank you for considering contributing to **AutoEntityGenerator**!  
Whether it's fixing a bug, adding a feature, improving documentation, or reporting an issue — your help is most welcome!  
By participating in this project, you agree to abide by our [Code of Conduct](CODE_OF_CONDUCT.md).

---

## 🛠️ How to Contribute

### 1. Choose an unassigned issue to work on from the open issues list.  
If you’d like to work on it, please comment on the issue — and a maintainer will assign it to you.  
This helps avoid duplicate work and makes tracking progress easier.

### 2. Fork the repository.

Click “Fork” at the top right of this page and clone your fork locally:

    git clone https://github.com/YOUR_USERNAME/AutoEntityGenerator.git
    cd AutoEntityGenerator

### 3. Create a new branch.

    git checkout -b feature/your-feature-name

### 4. Make your changes.

Be sure to:
- Follow the existing code style and conventions.
- Add unit tests for any new functionality.
- Keep code coverage high (95%+ preferred).

### 5. Run tests.

    nuget restore AutoEntityGenerator.sln
    msbuild AutoEntityGenerator.sln /p:Configuration=Release
    dotnet test --no-build --configuration Release

### 6. Commit your changes. 

Use clear and descriptive commit messages,  
Prefer multiple small, focused commits over a single large one.

    git commit -m "Add feature: support for XYZ"

### 7. Push your commit(s) and open a pull request.

    git push origin feature/your-feature-name

Make sure your code compiles and automation tests pass locally.

Then, open a pull request targeting the `main` branch.
Be sure to give it a meaningful name and a description. 

---

## 📦 Contribution Guidelines

- **No direct pushes to `main`** — all changes must go through a PR.
- Pull Requests must:
  - Be reviewed by a Code Owner (currently: @Peled-Zohar).
  - Pass all CI/CD checks (build + tests + code coverage).
  - Resolve any review comments before merging.
- Code should be readable, consistent, and maintainable.
- User-facing changes should be documented where applicable.

---

## 🧪 Running Tests Locally

    nuget restore AutoEntityGenerator.sln
    msbuild AutoEntityGenerator.sln /p:Configuration=Release
    dotnet test --no-build --configuration Release

Test coverage is reported via Codecov.

---

## 🧭 Suggested Areas for Contribution

- Bug fixes or reporting issues
- UX enhancements or usability suggestions
- New features (check `TODO.md` or GitHub issues)
- Additional unit or integration tests
- Improvements to CI/CD workflow

---

## 📞 Need Help?

Feel free to [open a discussion](https://github.com/Peled-Zohar/AutoEntityGenerator/discussions) or [file an issue](https://github.com/Peled-Zohar/AutoEntityGenerator/issues) if you have questions, suggestions, or just want to say hi.

---

Looking forward to your contributions!
