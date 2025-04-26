<script lang="ts">
	import Router from 'svelte-spa-router'
	
	import Home from "./routes/Home.svelte";
	import Profile from "./routes/Profile.svelte";
	import NavPartial from "./components/Nav.svelte";
	let name: string = "Tommy";


	// fetch name from backend
	// name = fetch("get")

	const routes = {
		"/": Home,
		"/:username": Profile,
	}; 




</script>

<NavPartial />

<Router {routes} />

<style>
	:global(body) {
		height: 100vh;
	}
</style>
