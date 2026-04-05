import axios from "axios";

let token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMjNrc2hpdGlqLm5lZ2lAZ21haWwuY29tIiwianRpIjoiNGI1YmFlY2MtNDUxNS00YzFiLTgyZmUtNjI4NTRkMzRiNTAzIiwiZXhwIjoxNzU3MTEyMjAyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDMzNyIsImF1ZCI6IlVzZXJzIn0.YgrcxlSG_QPY-oE2CJ-OSJzOnmpdqwFIX6EUvkw7xZM"

const api = axios.create({
  baseURL: 'https://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`
  }
});

let handleError = (error) => {
  if (error.response && error.response.data) {
    throw error.response.data;
  }
  throw error;
}

export const getMovies = async () => {
  try {
    return await api.get('/movies', {
      params: {
        home: true,
      }
    });
  }
  catch (error) {
    handleError(error);
  }
}

export const getMovieById = async (id) => {
  try {
    return await api.get(`/movies/${id}`);
  }
  catch (error) {
    handleError(error);
  }
}

export const createMovie = async (payload) => {
  try {
    return await api.post(`/movies`, payload);
  }
  catch (error) {
    handleError(error);
  }
}

export const updateMovieCover = async (id, file) => {
  try {
    const formData = new FormData();
    formData.append('img', file);

    return await api.put(`/movies/${id}/cover-image`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  } catch (error) {
    handleError(error);
  }
}

export const editMovie = async (id, payload) => {
  try {
    return await api.put(`/movies/${id}`, payload);
  }
  catch (error) {
    handleError(error);
  }
}

export const deleteMovie = async (id) => {
  try {
    return await api.delete(`/movies/${id}`);
  }
  catch (error) {
    handleError(error);
  }
}
export const getActors = async () => {
  try {
    return await api.get('/actors');
  }
  catch (error) {
    handleError(error);
  }
}

export const addActor = async (payload) => {
  try {
    return await api.post('/actors', payload);
  }
  catch (error) {
    handleError(error);
  }
}

export const getProducers = async () => {
  try {
    return await api.get('/producers');
  }
  catch (error) {
    handleError(error);
  }
}

export const addProducer = async (payload) => {
  try {
    return await api.post('/producers', payload);
  }
  catch (error) {
    handleError(error);
  }
}

export const getGenres = async () => {
  try {
    return await api.get('/genres');
  }
  catch (error) {
    handleError(error);
  }
}
