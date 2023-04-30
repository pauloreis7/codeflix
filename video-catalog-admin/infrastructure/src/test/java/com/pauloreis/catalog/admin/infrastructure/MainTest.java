package com.pauloreis.catalog.admin.infrastructure;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import org.springframework.core.env.AbstractEnvironment;

public class MainTest {
    @Test
    public void testMain() {
        System.setProperty(AbstractEnvironment.ACTIVE_PROFILES_PROPERTY_NAME, "test");
        assertNotNull(new Main());

        Main.main(new String[] {});
    }
}
