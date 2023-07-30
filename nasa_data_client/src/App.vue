<template>
  <v-app>
    <v-main>
      <meteor-table />
    </v-main>
  </v-app>
</template>

<script>
import MeteorTable from './components/meteor-table'

export default {
  name: 'App',

  components: {
    'meteor-table': MeteorTable,
  },
  data: () => ({
    //
  }),
}
</script>
