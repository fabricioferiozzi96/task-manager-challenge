import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { TaskListScreen } from '../../features/tasks/ui/screens/TaskListScreen';
import { TaskDetailScreen } from '../../features/tasks/ui/screens/TaskDetailScreen';
import { FiltersScreen } from '../../features/tasks/ui/screens/FiltersScreen';
import { colors } from '../theme/tokens';
import type { RootStackParamList } from './types';

const Stack = createNativeStackNavigator<RootStackParamList>();

export const RootNavigator: React.FC = () => (
  <NavigationContainer>
    <Stack.Navigator
      screenOptions={{
        headerStyle: { backgroundColor: colors.background },
        headerShadowVisible: false,
        headerTitleStyle: { fontWeight: '700' },
        contentStyle: { backgroundColor: colors.background },
      }}
    >
      <Stack.Screen
        name="TaskList"
        component={TaskListScreen}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name="TaskDetail"
        component={TaskDetailScreen}
        options={{ title: 'Detalle' }}
      />
      <Stack.Screen
        name="Filters"
        component={FiltersScreen}
        options={{ title: 'Filtros' }}
      />
    </Stack.Navigator>
  </NavigationContainer>
);
