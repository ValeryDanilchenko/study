import axios, {isCancel, AxiosError} from 'axios';

const API_URL = "https://api.hh.ru/vacancies";

async function fetchJobVacancies(searchText) {
  try {
    const response = await axios.get(API_URL, {
      params: {
        text: searchText,
      },
    });
    return response.data.items;
  } catch (error) {
    console.error("Error fetching job vacancies:", error);
    return [];
  }
}

function createJobVacancyElement(vacancy) {
    const vacancyElement = document.createElement("div");
    vacancyElement.className = "job-vacancy";
  
    const titleElement = document.createElement("h3");
    titleElement.textContent = vacancy.name;
    vacancyElement.appendChild(titleElement);
  
    const linkElement = document.createElement("a");
    linkElement.href = vacancy.alternate_url;
    linkElement.textContent = "Подробнее";
    linkElement.target = "_blank";
    vacancyElement.appendChild(linkElement);
  
    return vacancyElement;
  }
  
  function displayJobVacancies(vacancies, searchText) {
    const infoBlock = document.getElementById("widget");
  
    const container = document.createElement("div");
    container.className = "job-vacancies-container";
  
    const headerElement = document.createElement("h2");
    headerElement.textContent = `Ниже представлены вакансии по запросу: ${searchText}`;
    container.appendChild(headerElement);
  
    vacancies.forEach((vacancy) => {
      const vacancyElement = createJobVacancyElement(vacancy);
      container.appendChild(vacancyElement);
    });
  
    infoBlock.appendChild(container);
  }

async function searchJobVacancies() {
    const searchText = "JavaScript";
    const vacancies = await fetchJobVacancies(searchText);
    displayJobVacancies(vacancies, searchText);
  }
  
  searchJobVacancies();
  