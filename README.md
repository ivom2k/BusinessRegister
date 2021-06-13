# CLI program to check Business Register data

Hobby project

[Business Register OpenData Link](https://avaandmed.eesti.ee/datasets/ariregistri-avalikud-tasuta-andmed)

| Project | Purpose |
| ------- | ------- |
| BusinessRegister | Run the thing |
| DataFile | Download .csv from OpenData |
| Repository | [DTO](Repository/Company.cs) and [Repository](Repository/Repository.cs) for information access |

- [x] Get the file, save it, extract it
- [ ] Remove exceptions so the program keeps working and uses the current .csv file
- [ ] Log occuring errors so user knows what's up
- [ ] Read from it
- [ ] Create repository and DTO
- [ ] Allow query using business code
- [ ] Allow query using words
- [ ] Allow to copy business register link