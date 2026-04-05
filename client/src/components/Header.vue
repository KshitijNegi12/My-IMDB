<template>
  <header>
    <nav>
      <div class="nav-left">
        <router-link to="/">
          <img src="../assets/nav-img.png" alt="IMDB" width="70%" />
        </router-link>
        <h2>IMDB</h2>
      </div>
      <div class="nav-right">
        <router-link to="/">Movies</router-link>
        <router-link to="/actors">Actors</router-link>
        <router-link to="/producers">Producers</router-link>
      </div>
    </nav>
  </header>
</template>

<script>
export default {
  name: "AppHeader",
};
</script>

<style scoped>
nav {
  display: flex;
  justify-content: space-between;
  background-color: rgb(243, 243, 243);
}
.nav-left {
  margin-top: 0.4em;
  margin-left: 0.8em;
  display: flex;
  align-items: center;
}
.nav-right {
  gap: 10px;
  margin-top: 0.4em;
  margin-right: 0.8em;
  display: flex;
  align-items: center;
}
.nav-right > a {
  padding: 7px;
  border-radius: 10px;
  text-decoration: none;
  font-size: 1.2em;
  color: black;
  font-weight: 500;
}
.nav-right > a:hover {
  background-color: black;
  color: white;
}
@media (width <= 550px) {
  .nav-right {
    display: none;
  }
}
</style>
