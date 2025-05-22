<script lang="ts">
	import Router from "svelte-spa-router";
	import { modalType } from "./stores/modal";
	import Home from "./routes/Home.svelte";
	import Profile from "./routes/Profile.svelte";
	import NavPartial from "./components/Nav.svelte";
	import Signup from "./components/Signup.svelte";
	import Login from "./components/Login.svelte";
	let name: string = "Tommy";

	// fetch name from backend
	// name = fetch("get")

	const routes = {
		"/": Home,
		"/:username": Profile,
	};

	let modal = false;

	function displayModal(type: string) {
		modal = true;
		modalType.set(type);
	}
</script>

<NavPartial {displayModal} />

<main class="main-con">
	<Router {routes} />
	{#if modal}
		<div class="modal-focus"></div>
		<div class="modal-con">
			{#if $modalType === "signup"}
				<Signup />
			{:else if $modalType === "login"}
				<Login />
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
