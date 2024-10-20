<notifications>
</notifications>

<script lang="js">
import { defineComponent } from 'vue';

export default defineComponent({
    data() {
        return {
            result: null
        };
    },
    async mounted() {
        result = await (await fetch('weatherforecast')).json();
    }
});
</script>

<style scoped></style>