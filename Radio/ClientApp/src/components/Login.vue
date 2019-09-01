<template>
  <div class="root">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Login</h5>
        <form @submit="login">
          <div class="form-group">
            <label for="username">Username</label>
            <input
              type="text"
              class="form-control"
              :class="{ 'is-invalid': validity.username.isInvalid }"
              id="username"
              placeholder="Enter your username"
              v-model="username"
            />

            <div
              v-if="validity.username.isInvalid"
              class="invalid-feedback"
            >{{ errors.Username[0] }}</div>
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <input
              type="password"
              class="form-control"
              :class="{ 'is-invalid': validity.password.isInvalid }"
              id="password"
              placeholder="Enter your password"
              v-model="password"
            />

            <div
              v-if="validity.password.isInvalid"
              class="invalid-feedback"
            >{{ errors.Password[0] }}</div>
          </div>
          <button type="submit" class="btn btn-primary">Submit</button>
        </form>
        <hr/>
        <router-link class="card-link" :to="{ name: 'register', query: { redirect: $route.query.redirect || '/' } }">Register instead</router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
div.root {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

div.card {
  min-width: 300px;
}
</style>

<script>
import axios from "axios";

export default {
  props: ["redirect"],
  data() {
    return {
      username: "",
      password: "",
      errors: null,
      validity: {
        username: {
          isInvalid: false
        },
        password: {
          isInvalid: false
        }
      }
    };
  },
  async beforeRouteEnter(to, from, next) {
    if (await (await fetch("/user/is-logged-in")).json()) {
      next({
        path: from.path,
        replace: true
      });
    } else {
      next();
    }
  },
  methods: {
    login(event) {
      event.preventDefault();

      axios
        .post("/account/login", {
          Username: this.username,
          Password: this.password
        })
        .then((response) => {
          this.$emit("loggedIn");
          this.$router.push(this.redirect);
        })
        .catch((error) => {
          this.errors = error.response.data;

          for (const property of ["Username", "Password"]) {
            const isInvalid = this.errors.hasOwnProperty(property);

            const propertyValidity = this.validity[property[0].toLowerCase() + property.substring(1)];
            propertyValidity.isInvalid = isInvalid;
          }
        });
    }
  }
};
</script>