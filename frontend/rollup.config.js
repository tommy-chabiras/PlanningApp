import svelte from "rollup-plugin-svelte";
import resolve from "@rollup/plugin-node-resolve";
import postcss from "rollup-plugin-postcss";
import commonjs from "@rollup/plugin-commonjs";
import typescript from "@rollup/plugin-typescript";
import livereload from "rollup-plugin-livereload";
import terser from "@rollup/plugin-terser";
import { sveltePreprocess } from "svelte-preprocess";

const production = !process.env.ROLLUP_WATCH;

export default {
	input: "src/main.ts",
	output: {
		sourcemap: true,
		format: "iife",
		name: "app",
		file: "public/build/bundle.js",
	},
	onwarn(warning, warn) {
		if (warning.code === "CIRCULAR_DEPENDENCY") return; // Ignore circular dependency warnings
		warn(warning); // Default handling for other warnings
	},
	plugins: [

		svelte({
			include: ["src/**/*.svelte", "node_modules/**/*.svelte"],
			compilerOptions: {
				dev: !production,
				runes: true,
			},
			preprocess: sveltePreprocess(),
			emitCss: true,
		}),
		resolve({
			browser: true,
			dedupe: ["svelte"],
		}),
		postcss({
			extract: true,
			minimize: production,
			sourceMap: !production,
		}),
		commonjs(),
		typescript({
			sourceMap: !production,
			inlineSources: !production,
		}),
		!production && livereload("public"),
		production && terser(),
	],
	watch: {
		clearScreen: false,
	},
};
