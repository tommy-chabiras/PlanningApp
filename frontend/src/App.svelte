<script lang="ts">
	import Router from "svelte-spa-router";
	import { modal, modalType } from "./stores/modal";
	import Home from "./routes/Home.svelte";
	import Profile from "./routes/Profile.svelte";
	import NavPartial from "./components/Nav.svelte";
	import Signup from "./components/Signup.svelte";
	import SignupGuest from "./components/SignupGuest.svelte";
	import Login from "./components/Login.svelte";
	import CreatePlan from "./components/CreatePlan.svelte";

	const routes = {
		"/": Home,
		"/:username": Profile,
	};
</script>

<NavPartial />

<main class="main-con">
	<Router {routes} />
	{#if $modal}
		<div class="modal-focus"></div>
		<div class="modal-con">
			{#if $modalType === "signup"}
				<Signup />
			{:else if $modalType === "login"}
				<Login />
			{:else if $modalType === "create"}
				<CreatePlan />
			{:else if $modalType === "signupGuest"}
				<SignupGuest />
			{/if}
		</div>
	{/if}
</main>

<style>
	:global(*) {
		margin: 0;
		padding: 0;
		box-sizing: border-box;
	}

	:global(body) {
		height: 100vh;
		margin: 0px;
	}

	/* :global(.modal-focus) {
		height: 100%;
	} */
</style>
