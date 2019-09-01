<template>
  <div class="root">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Register</h5>
        <h6 class="card-subtitle mb-2 text-muted">Want to create your own channels? It's super easy! Just register so you can modify them later.</h6>
        <form @submit="register">
          <div class="form-group">
            <label for="username">Username</label>
            <input
              type="text"
              class="form-control"
              :class="{ 'is-valid': validity.username.isValid, 'is-invalid': validity.username.isInvalid }"
              id="username"
              placeholder="Enter a username"
              v-model="username"
            />

            <div v-if="validity.username.isValid" class="valid-feedback">Username is available!</div>
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
              :class="{ 'is-valid': validity.password.isValid, 'is-invalid': validity.password.isInvalid }"
              id="password"
              aria-describedby="password-help"
              placeholder="Enter a password"
              v-model="password"
            />
            <small
              id="password-help"
              class="form-text text-muted"
            >Your password is not stored verbatim.</small>

            <div v-if="validity.password.isValid" class="valid-feedback">Password is valid!</div>
            <div
              v-if="validity.password.isInvalid"
              class="invalid-feedback"
            >{{ errors.Password[0] }}</div>
          </div>
          <div class="form-group">
            <label for="repeat-password">Repeat Password</label>
            <input
              type="password"
              class="form-control"
              :class="{ 'is-valid': validity.repeatPassword.isValid, 'is-invalid': validity.repeatPassword.isInvalid }"
              id="repeat-password"
              placeholder="Repeat the above password"
              v-model="repeatPassword"
            />

            <div
              v-if="validity.repeatPassword.isValid"
              class="valid-feedback"
            >Repeat password matches!</div>
            <div
              v-if="validity.repeatPassword.isInvalid"
              class="invalid-feedback"
            >{{ errors.RepeatPassword[0] }}</div>
          </div>
          <button type="submit" class="btn btn-primary">Submit</button>
        </form>
        <hr/>
        <router-link class="card-link" :to="{ name: 'login', query: { redirect: $route.query.redirect || '/' } }">Login instead</router-link>
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
</style>

<script>
import axios from "axios";

export default {
  props: ["redirect"],
  data() {
    return {
      username: "",
      password: "",
      repeatPassword: "",
      errors: null,
      validity: {
        username: {
          isValid: false,
          isInvalid: false
        },
        password: {
          isValid: false,
          isInvalid: false
        },
        repeatPassword: {
          isValid: false,
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
    register(event) {
      event.preventDefault();

      axios
        .post("/account/register", {
          Username: this.username,
          Password: this.password,
          RepeatPassword: this.repeatPassword
        })
        .then((response) => {
          this.$emit("loggedIn");
          this.$router.push(this.redirect);
        })
        .catch((error) => {
          this.errors = error.response.data;

          for (const property of ["Username", "Password", "RepeatPassword"]) {
            const isInvalid = this.errors.hasOwnProperty(property);

            const propertyValidity = this.validity[
              property[0].toLowerCase() + property.substring(1)
            ];
            propertyValidity.isValid = !isInvalid;
            propertyValidity.isInvalid = isInvalid;
          }
        });
    }
  }
};
</script>