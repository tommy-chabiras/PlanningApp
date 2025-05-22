<script lang="ts">
	import { token } from "../stores/auth";
	import { displayModal } from "../stores/modal";


	function handleClick(type: string) {
		displayModal(type);
	}
	
</script>

<nav>
	<div>
		<a href="/"><h1>Link Up</h1></a>
	</div>
	{#if $token}
		<div class="desktop-menu">
			<a href="/create-plan">Create</a>
			<a href="/{$token.name.toLowerCase()}">{$token.name}</a>
		</div>
		<div class="mobile-menu">
			<a href="/{$token.name.toLowerCase()}">{$token.name}</a>
		</div>
	{:else}
		<div class="desktop-menu">
			<button on:click={() => handleClick("signup")}>Sign Up</button>
			<button on:click={() => handleClick("login")}>Log In</button>
		</div>
		<div class="mobile-menu">
			<button on:click={() => handleClick("signup")}>Sign Up</button>
			<button on:click={() => handleClick("login")}>Log In</button>
		</div>
	{/if}
</nav>

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
		width: 100%;
		box-shadow: 0 1px 5px 0px #00000078;
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
		background-color: rgba(200, 200, 236, 0.5);
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
