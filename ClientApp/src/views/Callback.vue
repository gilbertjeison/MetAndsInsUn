<template>
  <div>
    <p>Sign-in in progress</p>
  </div>
</template>
<script>
import store from "@/store/index";
import mgr from "@/security/security";
export default {
  async created() {
    try {
      console.log('antes del error...');
      var result = await mgr.signinRedirectCallback();
      var returnUrl = "home";
      // if (result.state !== undefined) {
      //   returnUrl = result.state;
      //   console.log('result.state :', result.state);
      // }
      this.$router.push({ name: returnUrl });
      console.log("No error ");
    } catch (error) {
      console.log("Error :", error);
      //this.$router.push({ name: "main" });
    }
  }
};
</script>