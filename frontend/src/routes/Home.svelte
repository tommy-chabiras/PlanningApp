<script lang="ts">
	import { onMount } from "svelte";
	import { modalType } from "../stores/modal";
	import CreatePlan from "../components/CreatePlan.svelte";
	// import EditPlan from "../components/EditPlan.svelte";

	let plans: any[];
	onMount(async () => {
		const response = await fetch("/api/user/get-plans", { method: "GET" });
		if (response.ok && (await response.text())) {
			plans = await response.json();
		}
	});

	let modal = false;

	function displayModal(type: string) {
		modal = true;
		modalType.set(type);
	}
</script>

<div class="plan-con flex">
	<button
		on:click={() => displayModal("create")}
		type="button"
		class="plan-card"
		aria-label="add plan"
	>
		<svg
			width="200px"
			height="200px"
			viewBox="0 0 24.00 24.00"
			fill="none"
			xmlns="http://www.w3.org/2000/svg"
			><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g
				id="SVGRepo_tracerCarrier"
				stroke-linecap="round"
				stroke-linejoin="round"
				stroke="#CCCCCC"
				stroke-width="0.4800000000000001"
			></g><g id="SVGRepo_iconCarrier">
				<path
					d="M15 12H12M12 12H9M12 12V9M12 12V15M17 21H7C4.79086 21 3 19.2091 3 17V7C3 4.79086 4.79086 3 7 3H17C19.2091 3 21 4.79086 21 7V17C21 19.2091 19.2091 21 17 21Z"
					stroke="#000000b0"
					stroke-width="1.2"
					stroke-linecap="round"
				></path>
			</g></svg
		>
	</button>
	{#each plans as plan, i}
		<div class="plan-card">
			<p>{plan.name}</p>
		</div>
	{/each}
</div>

{#if modal}
	{#if $modalType === "create"}
		<div class="modal-focus"></div>
		<div class="modal-con">
			<CreatePlan />
		</div>
	{/if}
{/if}
