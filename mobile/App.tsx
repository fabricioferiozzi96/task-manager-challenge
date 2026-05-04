import React from 'react';
import { SafeAreaView, StatusBar, Text, useColorScheme } from 'react-native';

const App: React.FC = () => {
  const isDarkMode = useColorScheme() === 'dark';

  return (
    <SafeAreaView style={{ flex: 1 }}>
      <StatusBar barStyle={isDarkMode ? 'light-content' : 'dark-content'} />
      <Text style={{ padding: 16, fontSize: 18 }}>Task Manager</Text>
    </SafeAreaView>
  );
};

export default App;
