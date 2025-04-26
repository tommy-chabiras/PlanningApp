<script lang="ts">
	import Signup from "./Signup.svelte";
	import Login from "./Login.svelte";

	let user: string = "";
	let modal: boolean = false;
	let modalType: string;

	function displayModal(type: string) {
		modal = true;
		modalType = type;
	}
</script>

<nav>
	<div>
		<a href="/"><h1>Link Up</h1></a>
	</div>
	{#if user}
		<div class="">
			<a href="/profile">Account</a>
			<a href="/create-plan">Create</a>
		</div>
		<div>
			<a href="/profile">Account</a>
		</div>
	{:else}
		<div class="desktop-menu">
			<button onclick={() => displayModal("signup")}>Sign Up</button>
			<button onclick={() => displayModal("login")}>Log In</button>
		</div>
		<div class="mobile-menu">
			<button onclick={() => displayModal("signup")}>Sign Up</button>
			<button onclick={() => displayModal("login")}>Log In</button>
		</div>
	{/if}
</nav>

{#if modal}
	{#if modalType === "signup"}
		<Signup />
	{:else if modalType === "login"}
		<Login />
	{/if}
{/if}

<style>
	:root {
		--nav-colour: hsl(0, 0%, 90%);
		--hover-colour: hsl(0, 0%, 80%);
	}

	nav {
		background-color: var(--nav-colour);
		padding: 10px 25px;
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	h1 {
		width: max-content;
		font-weight: 700;
		font-size: 1.5rem;
	}

	a {
		display: inline-block;
		padding: 5px 15px;
		border-radius: 10px;
	}

	a:hover {
		box-shadow: inset var(--hover-colour) 0px 0px 20px 5px;
		outline: 1px solid var(--nav-colour);
	}

	.desktop-menu button {
		padding: 10px 25px;
		background-color: rgb(200, 200, 236);
		border-radius: 10px;
	}
	
	.desktop-menu {
		display: block;
	}

	.mobile-menu {
		display: none;
	}

	@media screen and (max-width: 425px) {
		.desktop-menu {
			display: none;
		}

		.mobile-menu {
			display: block;
		}
	}
</style>
