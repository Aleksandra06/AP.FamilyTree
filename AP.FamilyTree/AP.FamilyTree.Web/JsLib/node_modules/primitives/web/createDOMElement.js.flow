// @flow

// Taken from:
// https://github.com/lelandrichardson/react-primitives/blob/master/src/Primitive.js
// https://github.com/necolas/react-native-web/blob/master/src/modules/createDOMElement/index.js

import React from 'react';

const roleComponents = {
  article: 'article',
  banner: 'header',
  button: 'button',
  complementary: 'aside',
  contentinfo: 'footer',
  form: 'form',
  heading: 'h1',
  link: 'a',
  list: 'ul',
  listitem: 'li',
  main: 'main',
  navigation: 'nav',
  region: 'section',
};

export type PrimitiveProps = {
  accessibilityLabel?: string,
  accessibilityLiveRegion?: 'assertive' | 'off' | 'polite',
  accessibilityRole?: $Keys<typeof roleComponents>,
  accessibilityLevel?: string | number,
  accessible?: boolean,
  testID?: string,
  type?: string,
};

function createDOMElement(
  component: string | Class<React.Component<*, *, *>>,
  props: PrimitiveProps = {},
) {
  const {
    accessibilityLabel,
    accessibilityLiveRegion,
    accessibilityRole,
    accessibilityLevel,
    accessible = true,
    testID,
    type,
    ...other
  } = props;

  const accessibilityComponent = accessibilityRole && roleComponents[accessibilityRole];
  const Component = accessibilityRole === 'heading' && accessibilityLevel
    ? headingComponent(accessibilityLevel)
    : accessibilityComponent || component;

  return <Component
    {...other}
    aria-hidden={accessible ? undefined : true}
    aria-label={accessibilityLabel}
    aria-live={accessibilityLiveRegion}
    aria-level={accessibilityLevel}
    data-testid={testID}
    role={accessibilityRole}
    type={accessibilityRole === 'button' ? 'button' : type}
  />;
}

export default createDOMElement;

function headingComponent(level: number | string) {
  // from h1 to h6
  return `h${Math.min(6, Math.max(1, parseInt(level, 10)))}`;
}
